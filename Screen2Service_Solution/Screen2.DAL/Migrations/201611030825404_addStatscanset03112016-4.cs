namespace Screen2.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addStatscanset031120164 : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.TradeSimulateOrders", "SetRef");
        }
        
        public override void Down()
        {
            AddColumn("dbo.TradeSimulateOrders", "SetRef", c => c.String());
        }
    }
}
