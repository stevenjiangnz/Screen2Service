namespace Screen2.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class updateTradeOrder111020162 : DbMigration
    {
        public override void Up()
        {
            //AddColumn("dbo.TradePositions", "EntryCost", c => c.Double(nullable: false));
            //AddColumn("dbo.TradePositions", "CurrentValue", c => c.Double(nullable: false));
            //AddColumn("dbo.TradePositions", "TradeSetId", c => c.Int(nullable: false));
            //CreateIndex("dbo.TradePositions", "TradeSetId");
            //AddForeignKey("dbo.TradePositions", "TradeSetId", "dbo.TradeSets", "Id", cascadeDelete: true);
            //DropColumn("dbo.TradePositions", "Fee");
            //DropColumn("dbo.TradePositions", "EntryTransactionId");
            //DropColumn("dbo.TradePositions", "ExitTransactionId");
        }
        
        public override void Down()
        {
            //AddColumn("dbo.TradePositions", "ExitTransactionId", c => c.Int());
            //AddColumn("dbo.TradePositions", "EntryTransactionId", c => c.Int(nullable: false));
            //AddColumn("dbo.TradePositions", "Fee", c => c.Double(nullable: false));
            //DropForeignKey("dbo.TradePositions", "TradeSetId", "dbo.TradeSets");
            //DropIndex("dbo.TradePositions", new[] { "TradeSetId" });
            //DropColumn("dbo.TradePositions", "TradeSetId");
            //DropColumn("dbo.TradePositions", "CurrentValue");
            //DropColumn("dbo.TradePositions", "EntryCost");
        }
    }
}
