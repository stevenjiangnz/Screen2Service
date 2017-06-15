namespace Screen2.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addStatscanset031120163 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.TradeSimulateOrders", "StatScanSetId", c => c.Int(nullable: false));
            CreateIndex("dbo.TradeSimulateOrders", "StatScanSetId");
            AddForeignKey("dbo.TradeSimulateOrders", "StatScanSetId", "dbo.StatScanSets", "Id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.TradeSimulateOrders", "StatScanSetId", "dbo.StatScanSets");
            DropIndex("dbo.TradeSimulateOrders", new[] { "StatScanSetId" });
            DropColumn("dbo.TradeSimulateOrders", "StatScanSetId");
        }
    }
}
