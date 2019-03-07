using System.Collections.Generic;
namespace BankATMRepositoryInterface
{
    public interface ITransaction
    {
        void InsertTransaction(Transaction transaction);
        
        IEnumerable<Transaction> ViewTopLatestTransactions(int accountID, int top);

        int GetTransactionCount(int accountID);
        
        void Save();

    }
}
