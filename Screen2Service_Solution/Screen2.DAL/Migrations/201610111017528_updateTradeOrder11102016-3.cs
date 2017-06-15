namespace Screen2.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class updateTradeOrder111020163 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.TradePositions", "TradeSetId", "dbo.TradeSets");
            DropIndex("dbo.TradePositions", new[] { "TradeSetId" });
            AddColumn("dbo.TradePositions", "Fee", c => c.Double(nullable: false));
            DropColumn("dbo.TradePositions", "EntryCost");
            DropColumn("dbo.TradePositions", "TradeSetId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.TradePositions", "TradeSetId", c => c.Int(nullable: false));
            AddColumn("dbo.TradePositions", "EntryCost", c => c.Double(nullable: false));
            DropColumn("dbo.TradePositions", "Fee");
            CreateIndex("dbo.TradePositions", "TradeSetId");
            AddForeignKey("dbo.TradePositions", "TradeSetId", "dbo.TradeSets", "Id", cascadeDelete: true);
        }
    }
}
