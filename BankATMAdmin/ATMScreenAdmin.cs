﻿using System;
using System.ComponentModel;

namespace BankATMAdmin
{
    public enum SecureMenuAdmin
    {
        // Value 1 is needed because menu starts with 1 while enum starts with 0 if no value given.

        [Description("Add Bank Account")]
        AddBankAccount = 1,

        [Description("Manage Bank Account")]
        ManageBankAccount = 2,

        [Description("Logout")]
        Logout = 3
    }

    class ATMScreenAdmin
    {
        public static void ShowMenu()
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

        public static void ShowMenuSecure()
        {
            Console.Clear();
            Console.WriteLine(" ---------------------------------");
            Console.WriteLine("| Meybank ATM Main Menu (Admin)  |");
            Console.WriteLine("|                                |");
            Console.WriteLine("| 1. Add Bank Account            |");
            Console.WriteLine("| 2. Manage Bank Account         |");
            Console.WriteLine("| 3. Logout                      |");
            Console.WriteLine("|                                |");
            Console.WriteLine(" ---------------------------------");
        }

        #region ATM UI Forms
        public static BankAccount getBankAccountForm()
        {
            var newBankAccountForm = new BankAccount();

            newBankAccountForm.FullName = Utility.GetValidStringInput("account full name");
            newBankAccountForm.NRIC = Utility.GetValidStringInput("NRIC");
            newBankAccountForm.AccountNumber = Utility.GetValidIntInputAmt("account number");
            newBankAccountForm.Balance = Utility.GetValidDecimalInputAmt("account starting balance");
            newBankAccountForm.CardNumber = Utility.GetValidIntInputAmt("card number");
            newBankAccountForm.PinCode = Utility.GetValidIntInputAmt("card pin");
            
            return newBankAccountForm;
        }
        #endregion
    }
}