using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankATMAdmin.Interface
{
    public interface IBankAccount
    {
        void AddBankAccount(BankAccount bankAccount);

        void UpdateBankAccount();

        void DeleteBankAccount(BankAccount bankAccount);

        void ViewBankAccount(BankAccount bankAccount);

        //void SearchBankAccount();

        void ManageBankAccount();
    }
}
