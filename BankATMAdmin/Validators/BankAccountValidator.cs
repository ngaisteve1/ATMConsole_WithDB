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
        public BankAccountValidator()
        {
            
            RuleFor(x => x.NRIC).Length(10, 14).WithMessage("Enter a valid NRIC length.");
            RuleFor(x => x.Balance).GreaterThanOrEqualTo(50).WithMessage($"Minimum starting balance required is {ATMScreenAdmin.cur} 50.00 to open new bank account.");
            RuleFor(x => x.PinCode).Must(x => x > 99999 && x < 1000000).WithMessage("Enter 6 digits for Pin code.");
            
        }

    }
}
