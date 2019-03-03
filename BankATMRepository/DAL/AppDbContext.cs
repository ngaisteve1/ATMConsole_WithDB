using System.Data.Entity;


public class AppDbContext : DbContext
    {
        public AppDbContext() : base("MeybankDB")
        {
            //Database.SetInitializer(new ATMDbInitializer());
        }

        public DbSet<BankAccount> BankAccounts { get; set; }
        public DbSet<Transaction> Transactions { get; set; }

    }

