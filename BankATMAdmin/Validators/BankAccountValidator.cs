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
            RuleFor(x => x.NRIC).Matches(@"^\d{6}-\d{2}-\d{4}$").WithMessage("Invalid NRIC format. Example of valid NRIC format, 900210-10-8080");
            RuleFor(x => x.NRIC).Must(isUniqueNRIC).WithMessage("Duplicate NRIC");
            //RuleFor(x => x.Balance).GreaterThanOrEqualTo(50).WithMessage($"A minimum of {ATMScreenAdmin.cur}50.00 balance is required to open Saving bank account type.");

            When(x => x.AccountType == AccountType.SavingAccount, () =>
                RuleFor(x => x.Balance)
                .GreaterThanOrEqualTo(50)
                .WithMessage($"A minimum of {ATMScreenAdmin.cur}50.00 balance is required to open Saving bank account type."));

            When(x => x.AccountType == AccountType.CurrentAccount, () =>
                RuleFor(x => x.Balance)
                .GreaterThanOrEqualTo(500)
                .WithMessage($"A minimum of {ATMScreenAdmin.cur}500.00 balance is required to open Current bank account type."));

            RuleFor(x => x.PinCode).Must(x => x > 99999 && x < 1000000).WithMessage("Enter 6 digits for ATM Card Pin code.");

        }

        private bool isUniqueNRIC(string _NRIC)
        {
            // If the new input NRIC matches with any of the NRIC in the database 
            bool NRICExist = db.BankAccounts.Any(b => b.NRIC.Equals(_NRIC));

            // return false. Otherwise, return true.
            return NRICExist ? false : true;
        }

    }
}
