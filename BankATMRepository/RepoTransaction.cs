using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankATMRepository
{
    public class RepoTransaction : ITransaction
    {
        private AppDbContext db = null;

        public RepoTransaction()
        {
            this.db = new AppDbContext();
        }

        public RepoTransaction(AppDbContext db)
        {
            this.db = db;
        }

        public void InsertTransaction(Transaction transaction)
        {
            db.Transactions.Add(transaction);
            Save();
        }

        public IEnumerable<Transaction> ViewTopLatestTransactions(long accountNumber, int top = 10)
        {
            return db.Transactions
                .Where(t => t.BankAccountNoFrom.Equals(accountNumber))
                .OrderByDescending(t => t.TransactionDate)
                .Take(top)
                .ToList();
        }

        public int GetTransactionCount(long accountNumber)
        {
            //return db.Transactions.Where(t => t.BankAccountNoFrom == accountNumber).Count();
            return db.Transactions.Count();
            
        }

        public void Save()
        {
            db.SaveChanges();
        }


    }
}
