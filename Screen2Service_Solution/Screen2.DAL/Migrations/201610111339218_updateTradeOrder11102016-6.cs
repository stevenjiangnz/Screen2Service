namespace Screen2.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class updateTradeOrder111020166 : DbMigration
    {
        public override void Up()
        {
            //AddColumn("dbo.Transactions", "TradeSetID", c => c.Int(nullable: false));
            //CreateIndex("dbo.Transactions", "TradeSetID");
            //AddForeignKey("dbo.Transactions", "TradeSetID", "dbo.TradeSets", "Id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            //DropForeignKey("dbo.Transactions", "TradeSetID", "dbo.TradeSets");
            //DropIndex("dbo.Transactions", new[] { "TradeSetID" });
            //DropColumn("dbo.Transactions", "TradeSetID");
        }
    }
}
