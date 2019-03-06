using System.Collections.Generic;

namespace BankATMRepository
{
    public interface IBankAccount
    {
        IEnumerable<BankAccount> ViewAllBankAccount();
        BankAccount ViewBankAccount(long accountNumber);
        void InsertBankAccount(BankAccount bankAccount);
        void DeleteBankAccount(BankAccount bankAccount);
        void Save();        
    }
}
