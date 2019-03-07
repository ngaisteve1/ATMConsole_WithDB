using System;

public class Transaction
{
    public int TransactionId { get; set; }
    public long? BankAccountNoFrom { get; set; }
    public long? BankAccountNoTo { get; set; }
    public TransactionType TransactionType { get; set; }
    public decimal TransactionAmount { get; set; }
    public DateTime TransactionDate { get; set; }
}

public enum TransactionType
{
    Deposit,
    Withdrawal,
    ThirdPartyTransfer
}