﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankATMRepository
{
    public class RepoBankAccount : IBankAccount
    {
        private AppDbContext db = null;

        public RepoBankAccount()
        {
            this.db = new AppDbContext();
        }

        public RepoBankAccount(AppDbContext db)
        {
            this.db = db;
        }

        public IEnumerable<BankAccount> ViewAllBankAccount()
        {
            return db.BankAccounts.ToList();   
        }

        public BankAccount ViewBankAccount(long accountNumber)
        {
            return db.BankAccounts.Where(b => b.AccountNumber.Equals(accountNumber)).SingleOrDefault();            
        }


        public void DeleteBankAccount(BankAccount bankAccount)
        {
            db.BankAccounts.Remove(bankAccount);
            Save();
        }

        public void InsertBankAccount(BankAccount bankAccount)
        {
            db.BankAccounts.Add(bankAccount);
            Save();
        }        

        public void Save()
        {            
            db.SaveChanges();
        }        
    }
}
