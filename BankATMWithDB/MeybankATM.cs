using ConsoleTables;
using System;
using System.Collections.Generic;
using System.Linq;
using BankATMRepo;
using BankATMRepository;

namespace MeybankATMSystem
{
    class MeybankATM : ILogin, IBalance, IDeposit, IWithdrawal, IThirdPartyTransfer
    {
        private static int tries;
        private const int maxTries = 3;
        private const decimal minimum_kept_amt = 20;

        //todo: A transaction class with transaction amount can replace these two variable.
        private static decimal transaction_amt;

        // No longer store data using list . Replace it with dbcontext ('virtual local db') object. 
       // private static List<BankAccount> _accountList;
        private static List<Transaction> _listOfTransactions;
        private static BankAccount selectedAccount;
        private static BankAccount inputAccount;


        // Connect to the database using db context object.
        private AppDbContext db = new AppDbContext();

        private IBankAccount repoBankAccount = null;
        private ITransaction repoTransaction = null;

        public MeybankATM()
        {
            this.repoBankAccount = new RepoBankAccount();
            this.repoTransaction = new RepoTransaction();
        }
        public MeybankATM(IBankAccount repoBankAccount, ITransaction repoTransaction)
        {
            this.repoBankAccount = repoBankAccount;
            this.repoTransaction = repoTransaction;
        }


        public void Execute()
        {
            //Initialization();
            ATMScreen.ShowMenu1();

            while (true)
            {
                switch (Utility.GetValidIntInputAmt("your option"))
                {
                    case 1:
                        CheckCardNoPassword();

                        _listOfTransactions = new List<Transaction>();

                        while (true)
                        {
                            ATMScreen.ShowMenu2();

                            switch (Utility.GetValidIntInputAmt("your option"))
                            {
                                case (int)SecureMenu.CheckBalance:
                                    CheckBalance(selectedAccount);
                                    break;
                                case (int)SecureMenu.PlaceDeposit:
                                    PlaceDeposit(selectedAccount);
                                    break;
                                case (int)SecureMenu.MakeWithdrawal:
                                    MakeWithdrawal(selectedAccount);
                                    break;
                                case (int)SecureMenu.ThirdPartyTransfer:
                                    var vMThirdPartyTransfer = new BankATMRepo.VMThirdPartyTransfer();
                                    vMThirdPartyTransfer = ATMScreen.ThirdPartyTransferForm();

                                    PerformThirdPartyTransfer(selectedAccount, vMThirdPartyTransfer);
                                    break;
                                case (int)SecureMenu.ViewTransaction:
                                    ViewTransaction();
                                    break;

                                case (int)SecureMenu.Logout:
                                    Utility.PrintMessage("You have succesfully logout. Please collect your ATM card..", true);

                                    Execute();
                                    break;
                                default:
                                    Utility.PrintMessage("Invalid Option Entered.", false);

                                    break;
                            }
                        }

                    case 2:
                        Console.Write("\nThank you for using Meybank. Exiting program now .");
                        Utility.printDotAnimation(15);

                        System.Environment.Exit(1);
                        break;
                    default:
                        Utility.PrintMessage("Invalid Option Entered.", false);
                        break;
                }
            }
        }

        private static void LockAccount()
        {
            Console.Clear();
            Utility.PrintMessage("Your account is locked.", true);
            Console.WriteLine("Please go to the nearest branch to unlocked your account.");
            Console.WriteLine("Thank you for using Meybank. ");
            Console.ReadKey();
            System.Environment.Exit(1);
        }

        public void Initialization()
        {
            transaction_amt = 0;

            // Move the data to the database via seeding

            // Without Entity Framework
            //_accountList = new List<BankAccount>
            //{
            //    new BankAccount() { FullName = "John", AccountNumber=333111, CardNumber = 123, PinCode = 111111, Balance = 2000.00m, isLocked = false },
            //    new BankAccount() { FullName = "Mike", AccountNumber=111222, CardNumber = 456, PinCode = 222222, Balance = 1500.30m, isLocked = true },
            //    new BankAccount() { FullName = "Mary", AccountNumber=888555, CardNumber = 789, PinCode = 333333, Balance = 2900.12m, isLocked = false }
            //};
        }

        public void CheckCardNoPassword()
        {
            bool pass = false;

            while (!pass)
            {
                inputAccount = new BankAccount();

                Console.WriteLine("\nNote: Actual ATM system will accept user's ATM card to validate");
                Console.Write("and read card number, bank account number and bank account status. \n\n");
               
                inputAccount.CardNumber = Utility.GetValidIntInputAmt("ATM Card Number");

                Console.Write("Enter 6 Digit PIN: ");
                inputAccount.PinCode = Convert.ToInt32(Utility.GetHiddenConsoleInput());
                // for brevity, length 6 is not validated and data type.


                Console.Write("\nChecking card number and password.");
                Utility.printDotAnimation();

                // LINQ Query
                var listOfAccounts = from a in db.BankAccounts
                                     select a;

                // Without Entity Framework
                //foreach (BankAccount account in _accountList)

                // With Entity Framework
                foreach (BankAccount account in listOfAccounts)
                {
                    if (inputAccount.CardNumber.Equals(account.CardNumber))
                    {
                        selectedAccount = account;

                        if (inputAccount.PinCode.Equals(account.PinCode))
                        {
                            if (selectedAccount.isLocked)
                                LockAccount();
                            else
                                pass = true;


                        }
                        else
                        {

                            pass = false;
                            tries++;

                            if (tries >= maxTries)
                            {
                                selectedAccount.isLocked = true;

                                LockAccount();
                            }

                        }
                    }
                }

                if (!pass)
                    Utility.PrintMessage("Invalid Card number or PIN.", false);

                Console.Clear();
            }
        }

        public void CheckBalance(BankAccount bankAccount)
        {
            Utility.PrintMessage($"Your bank account balance amount is: {Utility.FormatAmount(bankAccount.Balance)}", true);
        }

        public void PlaceDeposit(BankAccount account)
        {

            Console.WriteLine("\nNote: Actual ATM system will just let you ");
            Console.Write("place bank notes into ATM machine. \n\n");
            
            transaction_amt = Utility.GetValidDecimalInputAmt($"amount in {ATMScreen.cur}");

            System.Console.Write("\nCheck and counting bank notes.");
            Utility.printDotAnimation();

            if (transaction_amt <= 0)
                Utility.PrintMessage("Amount needs to be more than zero. Try again.", false);
            else if (transaction_amt % 10 != 0)
                Utility.PrintMessage($"Key in the deposit amount only with multiply of 10. Try again.", false);
            else if (!PreviewBankNotesCount(transaction_amt))
                Utility.PrintMessage($"You have cancelled your action.", false);
            else
            {
                // Bind transaction_amt to Transaction object
                // Add transaction record - Start
                var transaction = new Transaction()
                {
                    BankAccountNoFrom = account.AccountNumber,
                    BankAccountNoTo = account.AccountNumber,
                    TransactionType = TransactionType.Deposit,
                    TransactionAmount = transaction_amt,
                    TransactionDate = DateTime.Now
                };
                //InsertTransaction(transaction);
                repoTransaction.InsertTransaction(transaction);
                // Add transaction record - End

                // Another method to update account balance.
                account.Balance = account.Balance + transaction_amt;

                // Entity framework. To sync changes from dbcontext ('virtual local db') to physical db.
                db.SaveChanges();

                Utility.PrintMessage($"You have successfully deposited {Utility.FormatAmount(transaction_amt)}", true);
            }
        }

        public void MakeWithdrawal(BankAccount account)
        {
            Console.WriteLine("\nNote: For GUI or actual ATM system, user can ");
            Console.Write("choose some default withdrawal amount or custom amount. \n\n");
            
            transaction_amt = Utility.GetValidDecimalInputAmt($"amount {ATMScreen.cur}");

            if (transaction_amt <= 0)
                Utility.PrintMessage("Amount needs to be more than zero. Try again.", false);
            else if (transaction_amt > account.Balance)
                Utility.PrintMessage($"Withdrawal failed. You do not have enough fund to withdraw {Utility.FormatAmount(transaction_amt)}", false);
            else if ((account.Balance - transaction_amt) < minimum_kept_amt)
                Utility.PrintMessage($"Withdrawal failed. Your account needs to have minimum {Utility.FormatAmount(minimum_kept_amt)}", false);
            else if (transaction_amt % 10 != 0)
                Utility.PrintMessage($"Key in the deposit amount only with multiply of 10. Try again.", false);
            else
            {
                // Bind transaction_amt to Transaction object
                // Add transaction record - Start
                var transaction = new Transaction()
                {
                    BankAccountNoFrom = account.AccountNumber,
                    BankAccountNoTo = account.AccountNumber,
                    TransactionType = TransactionType.Withdrawal,
                    TransactionAmount = transaction_amt,
                    TransactionDate = DateTime.Now
                };
                //InsertTransaction(transaction);
                repoTransaction.InsertTransaction(transaction);
                // Add transaction record - End

                // Another method to update account balance.
                account.Balance = account.Balance - transaction_amt;

                // Entity framework. To sync changes from dbcontext ('virtual local db') to physical db.
                db.SaveChanges();

                Utility.PrintMessage($"Please collect your money. You have successfully withdraw {Utility.FormatAmount(transaction_amt)}", true);
            }
        }

        

        private static bool PreviewBankNotesCount(decimal amount)
        {
            int hundredNotesCount = (int)amount / 100;
            int fiftyNotesCount = ((int)amount % 100) / 50;
            int tenNotesCount = ((int)amount % 50) / 10;

            Console.WriteLine("\nSummary");
            Console.WriteLine("-------");
            Console.WriteLine($"{ATMScreen.cur} 100 x {hundredNotesCount} = {100 * hundredNotesCount}");
            Console.WriteLine($"{ATMScreen.cur} 50 x {fiftyNotesCount} = {50 * fiftyNotesCount}");
            Console.WriteLine($"{ATMScreen.cur} 10 x {tenNotesCount} = {10 * tenNotesCount}");
            Console.Write($"Total amount: {Utility.FormatAmount(amount)}\n\n");
            
            string opt = Utility.GetValidIntInputAmt("1 to confirm or 0 to cancel").ToString();

            return (opt.Equals("1")) ? true : false;
        }

        public void ViewTransaction()
        {

            //Without Entity Framework - if (_listOfTransactions.Count <= 0)
            if (db.Transactions.Count() <= 0)
                Utility.PrintMessage($"There is no transaction yet.", true);
            else
            {
                var table = new ConsoleTable("Id","Type", "From", "To", "Amount " + ATMScreen.cur, "Trans Date Time");

                // Without Entity Framework - foreach (var tran in _listOfTransactions)
                // With Entity Framework
                var transactionsOrder = (from t in db.Transactions
                                        orderby t.TransactionDate descending
                                        select t).Take(5); // SELECT Top 5
                                        
                foreach (var tran in transactionsOrder)
                {
                    table.AddRow(tran.TransactionId,tran.TransactionType, tran.BankAccountNoFrom, tran.BankAccountNoTo, tran.TransactionAmount,
                    tran.TransactionDate);
                }
                table.Options.EnableCount = false;
                table.Write();

                //Without Entity Framework - Utility.PrintMessage($"You have performed {_listOfTransactions.Count} transactions.", true);
                Utility.PrintMessage($"You have performed {db.Transactions.Count()} transactions.", true);
            }
        }

        //public void InsertTransaction(Transaction transaction)
        //{
        //    // Without Entity Framework - _listOfTransactions.Add(transaction);
        //    // With Entity Framework
        //    db.Transactions.Add(transaction);

        //    // With Entity Framework
        //    db.SaveChanges();
        //}

        public void PerformThirdPartyTransfer(BankAccount bankAccount, BankATMRepo.VMThirdPartyTransfer vMThirdPartyTransfer)
        {
            if (vMThirdPartyTransfer.TransferAmount <= 0)
                Utility.PrintMessage("Amount needs to be more than zero. Try again.", false);
            else if (vMThirdPartyTransfer.TransferAmount > bankAccount.Balance)
                // Check giver's account balance - Start
                Utility.PrintMessage($"Withdrawal failed. You do not have enough fund to withdraw {Utility.FormatAmount(transaction_amt)}", false);
            else if (bankAccount.Balance - vMThirdPartyTransfer.TransferAmount < 20)
                Utility.PrintMessage($"Withdrawal failed. Your account needs to have minimum {Utility.FormatAmount(minimum_kept_amt)}", false);
            // Check giver's account balance - End
            else
            {
                // Check if receiver's bank account number is valid.
                // Without Entity framework -
                //var selectedBankAccountReceiver = (from b in _accountList
                //                                   where b.AccountNumber == vMThirdPartyTransfer.RecipientBankAccountNumber
                //                                   select b).FirstOrDefault();

                // With Entity framework
                var selectedBankAccountReceiver = (from b in db.BankAccounts
                                                   where b.AccountNumber == vMThirdPartyTransfer.RecipientBankAccountNumber
                                                   select b).FirstOrDefault();

                if (selectedBankAccountReceiver == null)
                    Utility.PrintMessage($"Third party transfer failed. Receiver bank account number is invalid.", false);
                else if (selectedBankAccountReceiver.FullName != vMThirdPartyTransfer.RecipientBankAccountName)
                    Utility.PrintMessage($"Third party transfer failed. Recipient's account name does not match.", false);
                else
                {
                    // Bind transaction_amt to Transaction object
                    // Add transaction record - Start
                    Transaction transaction = new Transaction()
                    {
                        BankAccountNoFrom = bankAccount.AccountNumber,
                        BankAccountNoTo = vMThirdPartyTransfer.RecipientBankAccountNumber,
                        TransactionType = TransactionType.ThirdPartyTransfer,
                        TransactionAmount = vMThirdPartyTransfer.TransferAmount,
                        TransactionDate = DateTime.Now
                    };
                    // Without Entity framework - _listOfTransactions.Add(transaction);
                    // With Entity Framework
                    //db.Transactions.Add(transaction);
                    repoTransaction.InsertTransaction(transaction);

                    Utility.PrintMessage($"You have successfully transferred out {Utility.FormatAmount(vMThirdPartyTransfer.TransferAmount)} to {vMThirdPartyTransfer.RecipientBankAccountName}", true);
                    // Add transaction record - End

                    // Update balance amount (Giver)
                    bankAccount.Balance = bankAccount.Balance - vMThirdPartyTransfer.TransferAmount;

                    // Update balance amount (Receiver)
                    selectedBankAccountReceiver.Balance = selectedBankAccountReceiver.Balance + vMThirdPartyTransfer.TransferAmount;

                    // With Entity framework. To sync changes from dbcontext ('virtual local db') to physical db.
                    db.SaveChanges();
                }
            }
        }

       
    }
}
