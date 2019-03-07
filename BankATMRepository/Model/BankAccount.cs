using System.Collections;
using System.Collections.Generic;

public class BankAccount
{
    public int Id { get; set; }

    public string FullName { get; set; }

    public string NRIC { get; set; }

    public long AccountNumber { get; set; }
    public long CardNumber { get; set; }
    public long PinCode { get; set; }
    public decimal Balance { get; set; }

    public bool isLocked { get; set; } = false;

    // Navigation Property
    public ICollection<Transaction> Transactions { get; set; }
}

