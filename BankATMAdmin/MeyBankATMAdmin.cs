using BankATMAdmin.Interface;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BankATMAdmin
{
    class MeyBankATMAdmin : IBankAccount
    {
        private static AppDbContext db = new AppDbContext();

        public void Execute()
        {
            ATMScreenAdmin.ShowMenu();

            while (true)
            {
                switch (Utility.GetValidIntInputAmt("your option"))
                {
                    case 1:
                        Authentication();

                        while (true)
                        {
                            ATMScreenAdmin.ShowMenuSecure();

                            switch (Utility.GetValidIntInputAmt("your option"))
                            {
                                case (int)SecureMenuAdmin.AddSampleBankAccount:
                                    AddSampleBankAccount();
                                    break;
                                case (int)SecureMenuAdmin.AddBankAccount:
                                    AddBankAccount(ATMScreenAdmin.getBankAccountForm());
                                    break;
                                case (int)SecureMenuAdmin.ManageBankAccount:
                                    ManageBankAccount();

                                    break;
                                case (int)SecureMenuAdmin.Logout:
                                    Utility.PrintMessage("You have succesfully logout.", true);

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

        public bool Authentication()
        {
            bool pass = false;

            while (!pass)
            {
                string username, password;

                username = Utility.GetValidStringInput("Username");
                password = Utility.GetValidStringInput("Password");

                Console.Write("\nChecking username and password.");
                Utility.printDotAnimation();

                // LINQ Query
                if (username.Equals("admin") && password.Equals("abc123"))
                    pass = true;
                else
                    Utility.PrintMessage("Invalid Card number or PIN.", false);


                Console.Clear();
            }

            return pass;
        }

        #region CRUD Operation

        public void AddSampleBankAccount()
        {
            
            db.Database.ExecuteSqlCommand("DELETE FROM BankAccounts");
            db.Database.ExecuteSqlCommand("DELETE FROM Transactions");
            Utility.PrintMessage("Cleared alll data in the database.",false);

            var _accountList = new List<BankAccount>
            {
                new BankAccount() { FullName = "John", NRIC="901211-10-5600", AccountNumber=1333111, CardNumber = 123456789, PinCode = 111111, Balance = 2000.00m, isLocked = false },
                new BankAccount() { FullName = "Mike", NRIC="920211-12-6700", AccountNumber=5111222, CardNumber = 987654321, PinCode = 222222, Balance = 1500.30m, isLocked = true },
                new BankAccount() { FullName = "Mary", NRIC="930311-11-5900", AccountNumber=2888555, CardNumber = 999999999, PinCode = 333333, Balance = 2900.12m, isLocked = false }
            };
            try
            {

                foreach (var acct in _accountList)
                    db.BankAccounts.Add(acct);

                db.SaveChanges();
                Utility.PrintMessage($"{_accountList.Count} Sample bank account added successfully.", true);
            }
            finally
            {
                //Dispose();
            }

        }

        public void AddBankAccount(BankAccount _bankAccount)
        {

            try
            {
                db.BankAccounts.Add(_bankAccount);
                db.SaveChanges();
                Utility.PrintMessage("Bank account added successfully.", true);
            }
            finally
            {
                //Dispose();
            }

        }

        public void ManageBankAccount()
        {
            bool validAccount = false;

            // Create empty poco object to hold user input 
            var newBankAccount = new BankAccount();
            var selectedBankAccount = new BankAccount();

            while (!validAccount)
            {
                newBankAccount.AccountNumber = Utility.GetValidIntInputAmt("account number");

                // todo: architecture upgrade: move LINQ query to Repository layer. Call data by method and parameter.
                selectedBankAccount = (from b in db.BankAccounts
                                       where b.AccountNumber.Equals(newBankAccount.AccountNumber)
                                       select b).FirstOrDefault();

                if (selectedBankAccount != null)
                    validAccount = true;
                else
                    Utility.PrintMessage("Bank account not found.", false);
            }

            // If found selected bank account, view bank account details.
            ViewBankAccount(selectedBankAccount);
        }

        public void UpdateBankAccount()
        {
            Utility.PrintMessage("Update data feature is not available in this version", false);
            throw new NotImplementedException();
        }

        public void DeleteBankAccount(BankAccount _bankAccount)
        {
            // User Experience (UX)
            Console.Beep();

            // User Experience (UX)
            string opt2 = Utility.GetValidStringInput("Confirm delete? Yes (Y) or No (N)?");
            switch (opt2.ToUpper())
            {
                case "Y":
                    db.BankAccounts.Remove(_bankAccount);
                    db.SaveChanges();
                    Utility.PrintMessage("Selected bank account successfully deleted.", true);

                    break;
                case "N":
                    Console.WriteLine("Operation cancel.");
                    break;
                default:
                    Console.WriteLine("Invalid option.");
                    break;
            }
        }

        public void ViewBankAccount(BankAccount _bankAccount)
        {
            Console.WriteLine("Bank Account Details");
            Console.WriteLine("--------------------");
            Console.WriteLine($"FullName: {_bankAccount.FullName}");
            Console.WriteLine($"NRIC: {_bankAccount.NRIC}");
            Console.WriteLine($"BankAccountNumber: {_bankAccount.AccountNumber}");
            Console.WriteLine($"BankAccountBalance: {_bankAccount.Balance}");
            Console.WriteLine($"ATMCardNumber: {_bankAccount.CardNumber}");
            Console.WriteLine($"ATMPinNumber: ******");
            Console.WriteLine($"isLocked: {_bankAccount.isLocked}");
            Console.WriteLine();

            string opt = Utility.GetValidStringInput("Edit (E) or Delete (D) bank account?");
            switch (opt.ToUpper())
            {
                case "D":
                    DeleteBankAccount(_bankAccount);
                    break;
                case "E":
                    UpdateBankAccount();
                    break;
                default:
                    break;
            }
        }





        #endregion


    }
}
