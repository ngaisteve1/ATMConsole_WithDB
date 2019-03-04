namespace BankATMRepository.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Initial : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.BankAccounts",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        FullName = c.String(),
                        NRIC = c.String(),
                        AccountNumber = c.Long(nullable: false),
                        CardNumber = c.Long(nullable: false),
                        PinCode = c.Long(nullable: false),
                        Balance = c.Decimal(nullable: false, precision: 18, scale: 2),
                        isLocked = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Transactions",
                c => new
                    {
                        TransactionId = c.Int(nullable: false, identity: true),
                        BankAccountNoFrom = c.Long(nullable: false),
                        BankAccountNoTo = c.Long(nullable: false),
                        TransactionType = c.Int(nullable: false),
                        TransactionAmount = c.Decimal(nullable: false, precision: 18, scale: 2),
                        TransactionDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.TransactionId);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Transactions");
            DropTable("dbo.BankAccounts");
        }
    }
}
