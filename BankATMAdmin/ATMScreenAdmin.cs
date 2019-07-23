using BankATMAdmin.Validators;
using System;
using System.ComponentModel;
using FluentValidation.Results;
using BankATMRepository;

namespace BankATMAdmin
{
    public enum SecureMenuAdmin
    {
        // Value 1 is needed because menu starts with 1 while enum starts with 0 if no value given.

        [Description("Add 3 Sample Bank Account")]
        AddSampleBankAccount = 1,

        [Description("Add Bank Account")]
        AddBankAccount = 2,

        [Description("Manage Bank Account")]
        ManageBankAccount = 3,

        [Description("Logout")]
        Logout = 4
    }

    public class ATMScreenAdmin
    {
        // todo: to move to general library.
        internal static string cur = "RM ";

        private IMessagePrinter messagePrinter = null;
        
        public ATMScreenAdmin()
        {
            messagePrinter = new MockMessagePrinter();
        }

        public void ShowMenu()
        {
            Console.Clear();
            Console.WriteLine(" ---------------------------------");
            Console.WriteLine("| Meybank ATM Main Menu (Admin)  |");
            Console.WriteLine("|                                |");
            Console.WriteLine("| 1. Login                       |");
            Console.WriteLine("| 2. Exit Program                |");
            Console.WriteLine("|                                |");
            Console.WriteLine(" ---------------------------------");
        }

        public void ShowMenuSecure()
        {
            Console.Clear();
            Console.WriteLine(" ---------------------------------");
            Console.WriteLine("| Meybank ATM Main Menu (Admin)  |");
            Console.WriteLine("|                                |");
            Console.WriteLine("| 1. Add 3 Sample Bank Account   |");
            Console.WriteLine("| 2. Add Bank Account            |");
            Console.WriteLine("| 3. Manage Bank Account         |");
            Console.WriteLine("| 4. Logout                      |");
            Console.WriteLine("|                                |");
            Console.WriteLine(" ---------------------------------");
        }

        #region ATM UI Forms
        public BankAccount getBankAccountForm()
        {
            var newBankAccountForm = new BankAccount();

            BankAccountValidator bankAccountValidator = new BankAccountValidator();

            
            newBankAccountForm.FullName = Utility.Convert<string>("account full name");

            Console.WriteLine("Example of NRIC, 900210-10-8080");
            newBankAccountForm.NRIC = Utility.Convert<string>("NRIC");

            
            switch (Utility.Convert<string>("Account Type. (S)aving or (C)urrent account"))
            {
                case "S":
                    newBankAccountForm.AccountType = AccountType.SavingAccount;
                    break;
                case "C":
                    newBankAccountForm.AccountType = AccountType.CurrentAccount;
                    break;
                default:
                    break;
            }

            // Variation. Auto generate a random account number or from a running sequence number
            //newBankAccountForm.AccountNumber = Utility.GetValidIntInputAmt("account number");
            newBankAccountForm.AccountNumber = Utility.GenerateRandomNumber(7032040, 9834010, new Random());
              

            Console.WriteLine($"A minimum of {ATMScreenAdmin.cur}50.00 balance is required to open Saving bank account type.");
            newBankAccountForm.Balance = Utility.Convert<decimal>("account starting balance");

            // Variation. Auto generate a random account number or from a running sequence number
            //newBankAccountForm.CardNumber = Utility.GetValidIntInputAmt("ATM card number");
            newBankAccountForm.CardNumber = Utility.GenerateRandomNumber(203450123, 698910890, new Random());

            Console.WriteLine("Enter 6 digits for ATM Card Pin code.");
            newBankAccountForm.PinCode = Utility.Convert<long>("ATM card pin");

            ValidationResult validationResult = bankAccountValidator.Validate(newBankAccountForm);
            if (!validationResult.IsValid)
            {
                foreach (var failure in validationResult.Errors)
                {
                    // failure.PropertyName 
                    messagePrinter.PrintMessage($"Error: {failure.ErrorMessage}",false);    
                }

                if (!validationResult.IsValid)
                {
                    messagePrinter.PrintMessage($"Enter all Bank Account details again", false);
                    getBankAccountForm();
                }                
            }

            return newBankAccountForm;
        }


        #endregion
    }
}
