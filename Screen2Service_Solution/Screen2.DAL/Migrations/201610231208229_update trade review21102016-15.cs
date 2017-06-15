namespace Screen2.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class updatetradereview2110201615 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.TradeSimulateOrders", "Diff", c => c.Double());
            AlterColumn("dbo.TradeSimulateOrders", "ExitTradingDate", c => c.Int());
            AlterColumn("dbo.TradeSimulateOrders", "ExitPrice", c => c.Double());
            AlterColumn("dbo.TradeSimulateOrders", "Diff_Per", c => c.Double());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.TradeSimulateOrders", "Diff_Per", c => c.Double(nullable: false));
            AlterColumn("dbo.TradeSimulateOrders", "ExitPrice", c => c.Double(nullable: false));
            AlterColumn("dbo.TradeSimulateOrders", "ExitTradingDate", c => c.Int(nullable: false));
            DropColumn("dbo.TradeSimulateOrders", "Diff");
        }
    }
}
