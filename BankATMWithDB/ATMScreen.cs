using System;
using System.ComponentModel;
using System.Globalization;
using System.Text;
using System.Threading;

public enum SecureMenu
{
    // Value 1 is needed because menu starts with 1 while enum starts with 0 if no value given.

    [Description("Check balance")]
    CheckBalance = 1,

    [Description("Place Deposit")]
    PlaceDeposit = 2,

    [Description("Make Withdrawal")]
    MakeWithdrawal = 3,

    [Description("Third Party Transfer")]
    ThirdPartyTransfer = 4,
    
    [Description("Transaction")]
    ViewTransaction = 5,

    [Description("Change ATM Card PIN")]
    ChangeATMCardPIN = 6,

    [Description("Logout")]
    Logout = 7
}

public static class ATMScreen
{
    // todo: to move to general library.
    internal static string cur = "RM ";

    #region ATM UI Forms
    public static decimal DepositForm()
    {
        Console.WriteLine("\nNote: Actual ATM system will just let you ");
        Console.Write("place bank notes into ATM machine. \n\n");

        //return Utility.GetValidDecimalInputAmt($"amount {cur}");
        return Utility.Convert<decimal>($"amt {cur}");
    }

    public static decimal WithdrawalForm()
    {
        Console.WriteLine("\nNote: For GUI or actual ATM system, user can ");
        Console.Write("choose some default withdrawal amount or custom amount. \n\n");
        
        //return Utility.GetValidDecimalInputAmt($"amount {cur}");
        return Utility.Convert<decimal>($"amt {cur}");
    }

    public static BankATMRepo.VMThirdPartyTransfer ThirdPartyTransferForm(){
        var vMThirdPartyTransfer = new BankATMRepo.VMThirdPartyTransfer();

        vMThirdPartyTransfer.RecipientBankAccountNumber = Utility.Convert<long>("recipient's account number");

        //vMThirdPartyTransfer.TransferAmount = Utility.GetValidDecimalInputAmt($"amount {cur}");
        vMThirdPartyTransfer.TransferAmount = Utility.Convert<decimal>($"amount {cur}");


        //vMThirdPartyTransfer.RecipientBankAccountName = Utility.GetRawInput("recipient's account name");
        vMThirdPartyTransfer.RecipientBankAccountName = Utility.Convert<string>("recipient's account name");
        // no validation here yet.

        return vMThirdPartyTransfer;
    }


    #endregion

    #region UIOutput - ATM Menu

    public static void ShowMenu1()
    {
        Console.Clear();
        Console.WriteLine(" ------------------------");
        Console.WriteLine("| Meybank ATM Main Menu  |");
        Console.WriteLine("|                        |");
        Console.WriteLine("| 1. Insert ATM card     |");
        Console.WriteLine("| 2. Exit                |");
        Console.WriteLine("|                        |");
        Console.WriteLine(" ------------------------");
    }

    public static void ShowMenu2()
    {
        Console.Clear();
        Console.WriteLine(" ---------------------------");
        Console.WriteLine("| Meybank ATM Secure Menu    |");
        Console.WriteLine("|                            |");
        Console.WriteLine("| 1. Balance Enquiry         |");
        Console.WriteLine("| 2. Cash Deposit            |");
        Console.WriteLine("| 3. Withdrawal              |");
        Console.WriteLine("| 4. Third Party Transfer    |");
        Console.WriteLine("| 5. Transactions            |");
        Console.WriteLine("| 6. Change ATM Card PIN     |");
        Console.WriteLine("| 7. Logout                  |");
        Console.WriteLine("|                            |");
        Console.WriteLine(" ---------------------------");
    }
    #endregion
   
}