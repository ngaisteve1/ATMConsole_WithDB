using System.Collections.Generic;
namespace BankATMRepository
{
    public interface ITransaction
    {
        void InsertTransaction(Transaction transaction);

        //void ViewTransaction();

        IEnumerable<Transaction> ViewAllTransaction(long accountNumber);

        void Save();

    }
}
