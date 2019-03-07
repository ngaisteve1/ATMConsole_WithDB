using System.Collections.Generic;
namespace BankATMRepositoryInterface
{
    public interface ITransaction
    {
        void InsertTransaction(Transaction transaction);
        
        IEnumerable<Transaction> ViewTopLatestTransactions(long accountNumber, int top);

        int GetTransactionCount(long accountNumber);
        
        void Save();

    }
}
