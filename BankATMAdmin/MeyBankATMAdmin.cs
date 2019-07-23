using BankATMRepository;
using BankATMRepositoryInterface;
using log4net;
using log4net.Config;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace BankATMAdmin
{
    class MeyBankATMAdmin
    {
        private static AppDbContext db = new AppDbContext();
        private static readonly ILog _log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        private IBankAccount repoBankAccount = null;
        private IMessagePrinter messagePrinter = null;
                
        public MeyBankATMAdmin()
        {
            repoBankAccount = new RepoBankAccount();
            messagePrinter = new MockMessagePrinter();
        }

        public MeyBankATMAdmin(IBankAccount repoBankAccount)
        {
            this.repoBankAccount = repoBankAccount;
        }

        public void Execute()
        {
            CheckDB();
            ATMScreenAdmin aTMScreenAdmin = new ATMScreenAdmin();

            aTMScreenAdmin.ShowMenu();

            while (true)
            {
                switch (Utility.Convert<int>("your option"))
                {
                    case 1:
                        Authentication();

                        while (true)
                        {
                            aTMScreenAdmin.ShowMenuSecure();

                            switch (Utility.Convert<int>("your option"))
                            {
                                case (int)SecureMenuAdmin.AddSampleBankAccount:
                                    AddSampleBankAccount();
                                    break;
                                case (int)SecureMenuAdmin.AddBankAccount:
                                    AddBankAccount(aTMScreenAdmin.getBankAccountForm());
                                    break;
                                case (int)SecureMenuAdmin.ManageBankAccount:
                                    ManageBankAccount();

                                    break;
                                case (int)SecureMenuAdmin.Logout:
                                    messagePrinter.PrintMessage("You have succesfully logout.", true);

                                    Execute();
                                    break;
                                default:
                                    messagePrinter.PrintMessage("Invalid Option Entered.", false);

                                    break;
                            }
                        }

                    case 2:
                        Console.Write("\nThank you for using Meybank. Exiting program now .");
                        Utility.printDotAnimation(15);

                        System.Environment.Exit(1);
                        break;
                    default:
                        messagePrinter.PrintMessage("Invalid Option Entered.", false);
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

                username = Utility.Convert<string>("Username");
                password = Utility.Convert<string>("Password");

                Console.Write("\nChecking username and password.");
                Utility.printDotAnimation();

                // LINQ Query
                if (username.Equals("admin") && password.Equals("abc123"))
                    pass = true;
                else
                    messagePrinter.PrintMessage("Invalid username or password.", false);


                Console.Clear();
            }

            return pass;
        }

        #region CRUD Operation

        public void AddSampleBankAccount()
        {
            
            db.Database.ExecuteSqlCommand("DELETE FROM BankAccounts");
            db.Database.ExecuteSqlCommand("DELETE FROM Transactions");
            messagePrinter.PrintMessage("Cleared all data in the database.",false);

            var _accountList = new List<BankAccount>
            {
                new BankAccount() { FullName = "John", NRIC="901211-10-5600", AccountNumber=1333111, CardNumber = 123456789, PinCode = 111111, Balance = 2000.00m, isLocked = false },
                new BankAccount() { FullName = "Mike", NRIC="920211-12-6700", AccountNumber=5111222, CardNumber = 987654321, PinCode = 222222, Balance = 1500.30m, isLocked = true },
                new BankAccount() { FullName = "Mary", NRIC="930311-11-5900", AccountNumber=2888555, CardNumber = 999999999, PinCode = 333333, Balance = 2900.12m, isLocked = false }
            };
            try
            {

                foreach (var acct in _accountList)
                    // Without Repository layer
                    //db.BankAccounts.Add(acct);

                    // With Repository layer,
                    repoBankAccount.InsertBankAccount(acct);

                // Without Repository layer
                //db.SaveChanges();
                messagePrinter.PrintMessage($"{_accountList.Count} Sample bank account added successfully.", true);
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
                // Without Repository layer,
                //db.BankAccounts.Add(_bankAccount);
                //db.SaveChanges();

                // With Repository layer,
                repoBankAccount.InsertBankAccount(_bankAccount);

                messagePrinter.PrintMessage("Bank account added successfully.", true);
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
                newBankAccount.AccountNumber = Utility.Convert<int>("account number");

                // Without Repository layer
                //selectedBankAccount = (from b in db.BankAccounts
                //                       where b.AccountNumber.Equals(newBankAccount.AccountNumber)
                //                       select b).FirstOrDefault();

                // With Repository layer
                selectedBankAccount = repoBankAccount.ViewBankAccount(newBankAccount.AccountNumber);

                if (selectedBankAccount != null)
                    validAccount = true;
                else
                    messagePrinter.PrintMessage("Bank account not found.", false);
            }

            // If found selected bank account, view bank account details.
            ViewBankAccount(selectedBankAccount);
        }

        public void UpdateBankAccount()
        {
            messagePrinter.PrintMessage("Update data feature is not available in this version", false);
            throw new NotImplementedException();
        }

        public void DeleteBankAccount(BankAccount _bankAccount)
        {
            // User Experience (UX)
            Console.Beep();

            // User Experience (UX)
            string opt2 = Utility.Convert<string>("Delete bank account will delete all its bank transaction history data. Confirm delete? Yes (Y) or No (N)?");
            switch (opt2.ToUpper())
            {
                case "Y":
                    // Without Repository layer
                    //db.BankAccounts.Remove(_bankAccount);
                    //db.SaveChanges();

                    // With Repository layer
                    repoBankAccount.DeleteBankAccount(_bankAccount);

                    messagePrinter.PrintMessage("Selected bank account successfully deleted.", true);

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

            string opt = Utility.Convert<string>("Edit (E) or Delete (D) bank account?");
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

        public void CheckDB()
        {
            try
            {
                db.Database.Connection.Open();
            }
            catch (Exception)
            {
                Console.WriteLine("System Error. Please contact the bank.");
                XmlConfigurator.Configure();
                _log.Info("Database connection error. Check database services.");
                Environment.Exit(0);
                Console.ReadKey();
            }
            finally
            {
                db.Database.Connection.Close();
            }
        }
    }
}
