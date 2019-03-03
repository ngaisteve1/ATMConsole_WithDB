using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankATMWithDB.DAL
{
    class AppDbContext : DbContext
    {
        public AppDbContext() : base("MeybankDB")
        {
            //Database.SetInitializer(new ATMDbInitializer());
        }

        public DbSet<BankAccount> BankAccounts { get; set; }
        public DbSet<Transaction> Transactions { get; set; }

    }
}
