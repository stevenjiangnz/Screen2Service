namespace Screen2.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class updaterecord121120162 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.TradeOrders", "Reason", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.TradeOrders", "Reason");
        }
    }
}
