using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankATMRepository
{
    class ATMRepository : IRepository
    {
        private AppDbContext db = null;

        public ATMRepository()
        {
            this.db = new AppDbContext();
        }

        public ATMRepository(AppDbContext db)
        {
            this.db = db;
        }

        public IEnumerable<BankAccount> ViewAllBankAccount()
        {
            return db.BankAccounts.ToList();   
        }

        public BankAccount ViewBankAccount(long accountNumber)
        {
            throw new NotImplementedException();
        }


        public void DeleteBankAccount(BankAccount bankAccount)
        {
            throw new NotImplementedException();
        }

        public void InsertBankAccount(BankAccount bankAccount)
        {
            throw new NotImplementedException();
        }

        public void InsertTransaction()
        {
            throw new NotImplementedException();
        }

        public void Save()
        {
            throw new NotImplementedException();
        }

        

        public IEnumerable<Transaction> ViewAllTransaction()
        {
            throw new NotImplementedException();
        }


    }
}
