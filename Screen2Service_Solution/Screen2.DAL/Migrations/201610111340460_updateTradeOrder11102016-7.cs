namespace Screen2.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class updateTradeOrder111020167 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Transactions", "TradeSetID", "dbo.TradeSets");
            DropIndex("dbo.Transactions", new[] { "TradeSetID" });
            DropColumn("dbo.Transactions", "TradeSetID");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Transactions", "TradeSetID", c => c.Int(nullable: false));
            CreateIndex("dbo.Transactions", "TradeSetID");
            AddForeignKey("dbo.Transactions", "TradeSetID", "dbo.TradeSets", "Id", cascadeDelete: true);
        }
    }
}
