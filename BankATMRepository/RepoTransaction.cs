using BankATMRepositoryInterface;
using System;
using System.Collections.Generic;
using System.Linq;

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

        public IEnumerable<Transaction> ViewTopLatestTransactions(int accountID, int top = 10)
        {

            //return db.Transactions.Where(t => t.BankAccountNoFrom == accountNumber)
            //    .OrderByDescending(t => t.TransactionDate)
            //    .Take(top)
            //    .ToList();
            //var output = db.Transactions.Where(b => b.BankAccountNoFrom == accountNumber).AsEnumerable();
            var output = (from t in db.Transactions
                          where t.AccountID == accountID
                          orderby t.TransactionDate descending
                          select t).ToList().Take(top);
            return output;
        }

        public int GetTransactionCount(int accountId)
        {
            return db.Transactions.Where(t => t.AccountID == accountId).Count();
            //return db.Transactions.Count();
            
        }

        public void Save()
        {            
                db.SaveChanges();            
        }        

    }
}
