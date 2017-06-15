namespace Screen2.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class updateTradeOrder111020165 : DbMigration
    {
        public override void Up()
        {
            DropTable("dbo.TradePositionHistories");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.TradePositionHistories",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Size = c.Int(nullable: false),
                        EntryPrice = c.Double(nullable: false),
                        EntryCost = c.Double(nullable: false),
                        UpdateDT = c.DateTime(nullable: false),
                        ShareId = c.Int(nullable: false),
                        AccountId = c.Int(nullable: false),
                        TradeSetId = c.Int(nullable: false),
                        TransactionId = c.Int(nullable: false),
                        TradingDate = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
        }
    }
}
