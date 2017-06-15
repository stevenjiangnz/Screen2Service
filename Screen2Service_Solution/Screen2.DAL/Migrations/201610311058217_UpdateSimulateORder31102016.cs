namespace Screen2.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateSimulateORder31102016 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.TradeSimulateOrders", "ExitMode", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.TradeSimulateOrders", "ExitMode");
        }
    }
}
