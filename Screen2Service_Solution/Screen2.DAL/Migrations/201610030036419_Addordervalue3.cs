namespace Screen2.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Addordervalue3 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.TradeOrders", "OrderValue", c => c.Double(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.TradeOrders", "OrderValue");
        }
    }
}
