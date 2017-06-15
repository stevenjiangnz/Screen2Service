namespace Screen2.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class updatetradereview2110201616 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.TradeSimulateOrders", "Day5ProfitDays", c => c.Int());
            AddColumn("dbo.TradeSimulateOrders", "Day5Highest", c => c.Double());
            AddColumn("dbo.TradeSimulateOrders", "Day5Lowest", c => c.Double());
        }
        
        public override void Down()
        {
            DropColumn("dbo.TradeSimulateOrders", "Day5Lowest");
            DropColumn("dbo.TradeSimulateOrders", "Day5Highest");
            DropColumn("dbo.TradeSimulateOrders", "Day5ProfitDays");
        }
    }
}
