using BankATMAdmin.Interface;
using System;
using System.Collections.Generic;

namespace BankATMAdmin
{
    class MeyBankATMAdmin : IAddBankAccount
    {
        private AppDbContext db = new AppDbContext();

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
                                case (int)SecureMenuAdmin.AddBankAccount:
                                    AddBankAccount(ATMScreenAdmin.getBankAccountForm());                                   
                                    break;
                                case (int)SecureMenuAdmin.ManageBankAccount:
                                    
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

        public void AddBankAccount(BankAccount bankAccount)
        {
            db.BankAccounts.Add(bankAccount);
            db.SaveChanges();
            Utility.PrintMessage("Bank account added successfully.", true);
        }
    }
}
