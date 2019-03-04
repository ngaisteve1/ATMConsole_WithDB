
using System;
using System.ComponentModel.DataAnnotations;

public class BankAccount
{
    public int Id { get; set; }

    [Required]
    public string FullName { get; set; }

    //[Required]
    //[MaxLength(15, ErrorMessage ="Enter valid length")]
    public string NRIC { get; set; }

    public Int64 AccountNumber { get; set; }
    public Int64 CardNumber { get; set; }
    public Int64 PinCode { get; set; }
    public decimal Balance { get; set; }

    public bool isLocked { get; set; } = false;
}

