namespace Screen2.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class updateTradeOrder10102016 : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.TradeOrders", "OutstandingSize");
            DropColumn("dbo.TradeOrders", "FulfiledSize");
            DropColumn("dbo.TradeOrders", "OrderValue");
            DropColumn("dbo.TradeOrders", "IsFlip");
        }
        
        public override void Down()
        {
            AddColumn("dbo.TradeOrders", "IsFlip", c => c.Boolean(nullable: false));
            AddColumn("dbo.TradeOrders", "OrderValue", c => c.Double(nullable: false));
            AddColumn("dbo.TradeOrders", "FulfiledSize", c => c.Int(nullable: false));
            AddColumn("dbo.TradeOrders", "OutstandingSize", c => c.Int(nullable: false));
        }
    }
}
