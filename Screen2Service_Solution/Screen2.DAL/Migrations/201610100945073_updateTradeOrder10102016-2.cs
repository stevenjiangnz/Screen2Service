namespace Screen2.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class updateTradeOrder101020162 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.TradePositions", "TradeSetId", "dbo.TradeSets");
            DropForeignKey("dbo.Transactions", "TradeSetID", "dbo.TradeSets");
            DropIndex("dbo.TradePositions", new[] { "TradeSetId" });
            DropIndex("dbo.Transactions", new[] { "TradeSetID" });
            AddColumn("dbo.TradePositions", "Fee", c => c.Double(nullable: false));
            AddColumn("dbo.TradePositions", "EntryTransactionId", c => c.Int(nullable: false));
            AddColumn("dbo.TradePositions", "ExitTransactionId", c => c.Int());
            DropColumn("dbo.TradePositions", "EntryCost");
            DropColumn("dbo.TradePositions", "CurrentValue");
            DropColumn("dbo.TradePositions", "TradeSetId");
            DropColumn("dbo.Transactions", "TradeSetID");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Transactions", "TradeSetID", c => c.Int(nullable: false));
            AddColumn("dbo.TradePositions", "TradeSetId", c => c.Int(nullable: false));
            AddColumn("dbo.TradePositions", "CurrentValue", c => c.Double(nullable: false));
            AddColumn("dbo.TradePositions", "EntryCost", c => c.Double(nullable: false));
            DropColumn("dbo.TradePositions", "ExitTransactionId");
            DropColumn("dbo.TradePositions", "EntryTransactionId");
            DropColumn("dbo.TradePositions", "Fee");
            CreateIndex("dbo.Transactions", "TradeSetID");
            CreateIndex("dbo.TradePositions", "TradeSetId");
            AddForeignKey("dbo.Transactions", "TradeSetID", "dbo.TradeSets", "Id", cascadeDelete: true);
            AddForeignKey("dbo.TradePositions", "TradeSetId", "dbo.TradeSets", "Id", cascadeDelete: true);
        }
    }
}
