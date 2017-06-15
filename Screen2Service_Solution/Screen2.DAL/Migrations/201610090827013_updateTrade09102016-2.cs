namespace Screen2.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class updateTrade091020162 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.TradeOrders", "Source", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.TradeOrders", "Source");
        }
    }
}
