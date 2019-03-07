using System.Collections.Generic;

namespace BankATMRepositoryInterface
{
    public interface IBankAccount
    {
        IEnumerable<BankAccount> ViewAllBankAccount();
        BankAccount ViewBankAccount(long accountNumber);
        void InsertBankAccount(BankAccount bankAccount);
        void DeleteBankAccount(BankAccount bankAccount);
        void Save();

        //void AddBankAccount(BankAccount bankAccount);

        //void UpdateBankAccount();

        //void DeleteBankAccount(BankAccount bankAccount);

        //void ViewBankAccount(BankAccount bankAccount);

        ////void SearchBankAccount();

        //void ManageBankAccount();
    }
}
