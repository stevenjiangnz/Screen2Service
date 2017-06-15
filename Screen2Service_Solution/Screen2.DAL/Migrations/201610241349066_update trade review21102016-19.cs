namespace Screen2.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class updatetradereview2110201619 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.TradeSimulateOrders", "Day5AboveDays", c => c.Int());
            DropColumn("dbo.TradeSimulateOrders", "Day5ProfitDays");
        }
        
        public override void Down()
        {
            AddColumn("dbo.TradeSimulateOrders", "Day5ProfitDays", c => c.Int());
            DropColumn("dbo.TradeSimulateOrders", "Day5AboveDays");
        }
    }
}
