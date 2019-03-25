using BankATMRepository;
using BankATMRepositoryInterface;
using ConsoleTables;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace MeybankATMSystem
{
    public class MeybankATM : ILogin, IBalance, IDeposit, IWithdrawal, IThirdPartyTransfer
    {
        private static int tries;
        private const int maxTries = 3;
        private const decimal minimum_kept_amt = 20;

        //todo: A transaction class with transaction amount can replace these two variable.
        private decimal transaction_amt;

        // No longer store data using list . Replace it with dbcontext ('virtual local db') object. 
       // private static List<BankAccount> _accountList;
        //private static List<Transaction> _listOfTransactions;
        private static BankAccount selectedAccount;
        private static BankAccount inputAccount;


        // Connect to the database using db context object.
        //private AppDbContext db = new AppDbContext();
        private static AppDbContext ctx = new AppDbContext();

        private readonly IBankAccount repoBankAccount = null;
        private readonly ITransaction repoTransaction = null;
        private readonly IMessagePrinter _msgPrinter;

        public MeybankATM()
        {
            this.repoBankAccount = new RepoBankAccount();
            this.repoTransaction = new RepoTransaction();
            _msgPrinter = new MockMessagePrinter();
        }

        public MeybankATM(IBankAccount repoBankAccount, ITransaction repoTransaction, IMessagePrinter msgPrinter)
        {
            this.repoBankAccount = repoBankAccount;
            this.repoTransaction = repoTransaction;
            _msgPrinter = msgPrinter;
        }


        public void Execute()
        {
            ATMScreen.ShowMenu1();

            while (true)
            {
                switch (Utility.GetValidIntInputAmt("your option"))
                {
                    case 1:
                        CheckCardNoPassword();

                        //_listOfTransactions = new List<Transaction>();

                        while (true)
                        {
                            ATMScreen.ShowMenu2();

                            switch (Utility.GetValidIntInputAmt("your option"))
                            {
                                case (int)SecureMenu.CheckBalance:
                                    // Get
                                    CheckBalance(selectedAccount);
                                    break;
                                case (int)SecureMenu.PlaceDeposit:
                                    // Get
                                    transaction_amt = ATMScreen.DepositForm();

                                    // Post
                                    PlaceDeposit(selectedAccount, transaction_amt);
                                    break;
                                case (int)SecureMenu.MakeWithdrawal:
                                    // Get
                                    transaction_amt = ATMScreen.WithdrawalForm();

                                    // Post
                                    MakeWithdrawal(selectedAccount, transaction_amt);
                                    break;
                                case (int)SecureMenu.ThirdPartyTransfer:
                                    // Get
                                    var vMThirdPartyTransfer = new BankATMRepo.VMThirdPartyTransfer();

                                    // Post
                                    vMThirdPartyTransfer = ATMScreen.ThirdPartyTransferForm();

                                    PerformThirdPartyTransfer(selectedAccount, vMThirdPartyTransfer);
                                    break;
                                case (int)SecureMenu.ViewTransaction:
                                    ViewTransaction(selectedAccount.Id);
                                    break;
                                case (int)SecureMenu.ChangeATMCardPIN:
                                    _msgPrinter.PrintMessage("This function is not ready.", false);
                                    
                                    break;
                                case (int)SecureMenu.Logout:
                                    _msgPrinter.PrintMessage("You have succesfully logout. Please collect your ATM card.", true);

                                    Execute();
                                    break;
                                default:
                                    _msgPrinter.PrintMessage("Invalid Option Entered.", false);

                                    break;
                            }
                        }

                    case 2:
                        Console.Write("\nThank you for using Meybank. Exiting program now .");
                        Utility.printDotAnimation(15);

                        System.Environment.Exit(1);
                        break;
                    default:
                        _msgPrinter.PrintMessage("Invalid Option Entered.", false);
                        break;
                }
            }
        }

        public void LockAccount()
        {
            Console.Clear();
            _msgPrinter.PrintMessage("Your account is locked.", true);
            _msgPrinter.PrintMessage("Please go to the nearest branch to unlocked your account.",false);
            _msgPrinter.PrintMessage("Thank you for using Meybank. ", false);
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
                // Without repository layer
                //var listOfAccounts = from a in db.BankAccounts
                //                     select a;

                // With repository layer
                var listOfAccounts = repoBankAccount.ViewAllBankAccount();
                var validCardNo = (from acc in listOfAccounts
                                   where acc.CardNumber == inputAccount.CardNumber
                                   select acc).SingleOrDefault();

                var validAccount = (from acc in listOfAccounts
                                  where acc.CardNumber == inputAccount.CardNumber
                                  && acc.PinCode == inputAccount.PinCode
                                  select acc).SingleOrDefault();

                // Without Entity Framework
                //foreach (BankAccount account in _accountList)

                // With Entity Framework
                if(validCardNo != null) // valid Card Number
                {


                    // Check both card number and card pin.
                    if (validAccount != null)
                    {
                        if(validAccount.isLocked)
                        {
                            LockAccount();
                            pass = false;
                        }

                        else
                        {
                            selectedAccount = validAccount;
                            pass = true;
                        }
                            
                    }
                    else
                    {
                        // Check how many times user login tries.
                        pass = false;
                        tries++;

                        // If more than 3 tries, lock the account.
                        if (tries >= maxTries)
                        {
                            validCardNo.isLocked = true;
                            repoBankAccount.Save();
                            LockAccount();
                        }
                    }
                }
                else
                {
                    pass = false;
                }

                // Without LINQ                
                //foreach (BankAccount account in listOfAccounts)
                //{
                //    if (inputAccount.CardNumber.Equals(account.CardNumber))
                //    {
                //        selectedAccount = account;

                //        if (inputAccount.PinCode.Equals(account.PinCode))
                //        {
                //            if (selectedAccount.isLocked)
                //                LockAccount();
                //            else
                //                pass = true;                            
                //        }
                //        else
                //        {
                //            pass = false;
                //            tries++;

                //            if (tries >= maxTries)
                //            {
                //                selectedAccount.isLocked = true;

                //                LockAccount();
                //            }

                //        }
                //    }
                //}

                if (!pass)
                    _msgPrinter.PrintMessage("Invalid Card number or PIN.", false);

                Console.Clear();
            }
        }

        public void CheckBalance(BankAccount bankAccount)
        {
            _msgPrinter.PrintMessage($"Your bank account balance amount is: {Utility.FormatAmount(bankAccount.Balance)}", true);
        }

        public void PlaceDeposit(BankAccount account, decimal transaction_amt)
        {   
            //transaction_amt = Utility.GetValidDecimalInputAmt($"amount in {ATMScreen.cur}");

            Console.Write("\nCheck and counting bank notes.");
            Utility.printDotAnimation();

            if (transaction_amt <= 0)
                
                _msgPrinter.PrintMessage("Amount needs to be more than zero. Try again.", false);
            else if (transaction_amt % 10 != 0)
                _msgPrinter.PrintMessage($"Key in the deposit amount only with multiply of 10. Try again.", false);
            else if (!PreviewBankNotesCount(transaction_amt))
                _msgPrinter.PrintMessage($"You have cancelled your action.", false);
            else
            {
                // Bind transaction_amt to Transaction object
                // Add transaction record - Start
                var transaction = new Transaction()
                {
                    AccountID = account.Id,
                    BankAccountNoTo = account.AccountNumber,
                    TransactionType = TransactionType.CashDeposit,
                    TransactionAmount = transaction_amt,
                    Description = TransactionType.CashDeposit.ToString(),
                    TransactionDate = DateTime.Now
                };
                //InsertTransaction(transaction);
                repoTransaction.InsertTransaction(transaction);
                // Add transaction record - End

                // Another method to update account balance.
                account.Balance = account.Balance + transaction_amt;

                // Entity framework. To sync changes from dbcontext ('virtual local db') to physical db.
                ctx.SaveChanges();

                _msgPrinter.PrintMessage($"Please collect your bank slip. You have successfully deposited {Utility.FormatAmount(transaction_amt)}", true);
            }
        }

        public void MakeWithdrawal(BankAccount account, decimal _transaction_amt)
        {         
           // transaction_amt = Utility.GetValidDecimalInputAmt($"amount {ATMScreen.cur}");

            if (_transaction_amt <= 0)
                _msgPrinter.PrintMessage("Amount needs to be more than zero. Try again.", false);
            else if (_transaction_amt > account.Balance)
                _msgPrinter.PrintMessage($"Withdrawal failed. You do not have enough fund to withdraw {Utility.FormatAmount(_transaction_amt)}", false);
            else if ((account.Balance - _transaction_amt) < minimum_kept_amt)
                _msgPrinter.PrintMessage($"Withdrawal failed. Your account needs to have minimum {Utility.FormatAmount(minimum_kept_amt)}", false);
            else if (_transaction_amt % 10 != 0)
                _msgPrinter.PrintMessage($"Key in the deposit amount only with multiply of 10. Try again.", false);
            else
            {
                // Bind transaction_amt to Transaction object
                // Add transaction record - Start
                
                var transaction = new Transaction()
                {
                    AccountID = account.Id,
                    BankAccountNoFrom = account.AccountNumber,
                    TransactionType = TransactionType.CashWithdrawal,
                    TransactionAmount = Math.Abs(_transaction_amt) * (-1),
                    Description = TransactionType.CashWithdrawal.ToString(),
                    TransactionDate = DateTime.Now
                };
                //InsertTransaction(transaction);
                repoTransaction.InsertTransaction(transaction);
                // Add transaction record - End

                // Another method to update account balance.
                account.Balance = account.Balance + transaction.TransactionAmount;

                // Entity framework. To sync changes from dbcontext ('virtual local db') to physical db.
                ctx.SaveChanges();

                _msgPrinter.PrintMessage($"Please collect your money and bank slip. You have successfully withdraw {Utility.FormatAmount(_transaction_amt)}", true);
            }
        }

        

        private static bool PreviewBankNotesCount(decimal amount)
        {
            int hundredNotesCount = (int)amount / 100;
            int fiftyNotesCount = ((int)amount % 100) / 50;
            int tenNotesCount = ((int)amount % 50) / 10;

            Console.WriteLine("\nSummary");
            Console.WriteLine("-------");
            Console.WriteLine($"{ATMScreen.cur} 100 x {hundredNotesCount} \t= {100 * hundredNotesCount}");
            Console.WriteLine($"{ATMScreen.cur} 50 \tx {fiftyNotesCount} \t= {50 * fiftyNotesCount}");
            Console.WriteLine($"{ATMScreen.cur} 10 \tx {tenNotesCount} \t= {10 * tenNotesCount}");
            Console.Write($"Total amount: {Utility.FormatAmount(amount)}\n\n");
            
            string opt = Utility.GetValidIntInputAmt("1 to confirm or 0 to cancel").ToString();

            return (opt.Equals("1")) ? true : false;
        }

        public void ViewTransaction(int accountID)
        {

            //Without Entity Framework - if (_listOfTransactions.Count <= 0)
            // Before repository layer - db.Transactions.Count() 
            // After repository layer,
            //if (repoTransaction.GetTransactionCount(accountNumber) == 0)
            if (repoTransaction.GetTransactionCount(accountID) == 0)
                _msgPrinter.PrintMessage($"There is no transaction yet.", true);
            else
            {
                var table = new ConsoleTable("Date", "Description", "Amount");

                // Without Entity Framework - foreach (var tran in _listOfTransactions)

                // With Entity Framework
                // Without repository layer
                //var transactionsOrder = (from t in db.Transactions
                //                        orderby t.TransactionDate descending
                //                        select t).Take(5); // SELECT Top 5

                // With repository layer,

                //Console.WriteLine(repoTransaction.GetTransactionCount(accountID));

                foreach (var tran in repoTransaction.ViewTopLatestTransactions(accountID, 5))
                {
                    table.AddRow(tran.TransactionDate,tran.Description, Utility.FormatAmountTransaction(tran.TransactionAmount));
                }
                table.Options.EnableCount = false;
                table.Write();

                //Without Entity Framework - Utility.PrintMessage($"You have performed {_listOfTransactions.Count} transactions.", true);
                _msgPrinter.PrintMessage($"You have performed {repoTransaction.GetTransactionCount(accountID)} transactions.", true);
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
                _msgPrinter.PrintMessage("Amount needs to be more than zero. Try again.", false);
            else if (vMThirdPartyTransfer.TransferAmount > bankAccount.Balance)
                // Check giver's account balance - Start
                _msgPrinter.PrintMessage($"Withdrawal failed. You do not have enough fund to withdraw {Utility.FormatAmount(transaction_amt)}", false);
            else if (bankAccount.Balance - vMThirdPartyTransfer.TransferAmount < 20)
                _msgPrinter.PrintMessage($"Withdrawal failed. Your account needs to have minimum {Utility.FormatAmount(minimum_kept_amt)}", false);
            // Check giver's account balance - End
            else
            {
                // Check if receiver's bank account number is valid.
                // Without Entity framework -
                //var selectedBankAccountReceiver = (from b in _accountList
                //                                   where b.AccountNumber == vMThirdPartyTransfer.RecipientBankAccountNumber
                //                                   select b).FirstOrDefault();

                // With Entity framework
                // Without repository layer
                //var selectedBankAccountReceiver = (from b in db.BankAccounts
                //                                   where b.AccountNumber == vMThirdPartyTransfer.RecipientBankAccountNumber
                //                                   select b).FirstOrDefault();

                // With repository layer
                var selectedBankAccountReceiver = repoBankAccount.ViewBankAccount(vMThirdPartyTransfer.RecipientBankAccountNumber);

                if (selectedBankAccountReceiver == null)
                    _msgPrinter.PrintMessage($"Third party transfer failed. Receiver bank account number is invalid.", false);
                else if (selectedBankAccountReceiver.FullName != vMThirdPartyTransfer.RecipientBankAccountName)
                    _msgPrinter.PrintMessage($"Third party transfer failed. Recipient's account name does not match.", false);
                else
                {
                    // Bind transaction_amt to Transaction object
                    // Add transaction record - Start
                    Transaction transaction = new Transaction()
                    {
                        AccountID = bankAccount.Id,
                        BankAccountNoFrom = bankAccount.AccountNumber,
                        BankAccountNoTo = vMThirdPartyTransfer.RecipientBankAccountNumber,
                        TransactionType = TransactionType.ThirdPartyTransfer,
                        TransactionAmount = Math.Abs(vMThirdPartyTransfer.TransferAmount) * (-1),
                        Description = TransactionType.ThirdPartyTransfer.ToString() + " to bank account number " + vMThirdPartyTransfer.RecipientBankAccountNumber,
                        TransactionDate = DateTime.Now
                    };

                    Transaction transaction2 = new Transaction()
                    {
                        AccountID = selectedBankAccountReceiver.Id,
                        BankAccountNoFrom = bankAccount.AccountNumber,
                        BankAccountNoTo = vMThirdPartyTransfer.RecipientBankAccountNumber,
                        TransactionType = TransactionType.ThirdPartyTransfer,
                        TransactionAmount = vMThirdPartyTransfer.TransferAmount,
                        Description = TransactionType.ThirdPartyTransfer.ToString() + " from bank account number " + bankAccount.AccountNumber,
                        TransactionDate = DateTime.Now
                    };

                    // Without Entity framework - _listOfTransactions.Add(transaction);
                    // With Entity Framework
                    //db.Transactions.Add(transaction);
                    repoTransaction.InsertTransaction(transaction);
                    repoTransaction.InsertTransaction(transaction2);
                    // Add transaction record - End

                    // Update balance amount (Giver)
                    bankAccount.Balance += vMThirdPartyTransfer.TransferAmount;

                    // Update balance amount (Receiver)
                    selectedBankAccountReceiver.Balance += vMThirdPartyTransfer.TransferAmount;

                    // With Entity framework. To sync changes from dbcontext ('virtual local db') to physical db.
                    ctx.SaveChanges();

                    _msgPrinter.PrintMessage($"Please collect your bank slip. You have successfully transferred out {Utility.FormatAmount(vMThirdPartyTransfer.TransferAmount)} to {vMThirdPartyTransfer.RecipientBankAccountName}", true);
                }
            }
        }    
    }
}
