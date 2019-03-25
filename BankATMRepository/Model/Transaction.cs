using System;
using System.Collections.Generic;

public class Transaction
{
    public int TransactionId { get; set; }
    public long? BankAccountNoFrom { get; set; }
    public long? BankAccountNoTo { get; set; }
    public TransactionType TransactionType { get; set; }
    public decimal TransactionAmount { get; set; }
    public string Description { get; set; }
    public DateTime TransactionDate { get; set; }

    // Foreign Key
    public int AccountID { get; set; }

    //  Navigation Property
    public BankAccount bankAccount { get; set; }
}

public enum TransactionType
{
    CashDeposit,
    CashWithdrawal,
    ThirdPartyTransfer
}