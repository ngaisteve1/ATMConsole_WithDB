using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankATMAdmin.Validators
{
    public class BankAccountValidator : AbstractValidator<BankAccount>
    {
        private AppDbContext db = new AppDbContext();

        public BankAccountValidator()
        {
            
            RuleFor(x => x.NRIC).Length(10, 14).WithMessage("Enter a valid NRIC length.");
            RuleFor(x => x.AccountNumber).Must(isUniqueAccountNumber).WithMessage("Duplicate account number");
            RuleFor(x => x.Balance).GreaterThanOrEqualTo(50).WithMessage($"Minimum starting balance required is {ATMScreenAdmin.cur} 50.00 to open new bank account.");
            RuleFor(x => x.PinCode).Must(x => x > 99999 && x < 1000000).WithMessage("Enter 6 digits for Pin code.");
            
        }

        private bool isUniqueAccountNumber(Int64 _accountNumber)
        {
            // If the new input account number matches with any of the account number in the database 
            bool accountNumberExist = db.BankAccounts.Any(b => b.AccountNumber.Equals(_accountNumber));
            
            // return false. Otherwise, return true.
            return accountNumberExist ? false : true;
        }
    }
}
