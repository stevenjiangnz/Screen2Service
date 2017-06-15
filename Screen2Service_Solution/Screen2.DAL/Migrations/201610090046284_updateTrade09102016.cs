namespace Screen2.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class updateTrade09102016 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AccountBalanceJourneys", "TradeSetId", c => c.Int());
            AddColumn("dbo.TradeOrders", "IsFlip", c => c.Boolean(nullable: false));
            AddColumn("dbo.TradePositions", "Source", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.TradePositions", "Source");
            DropColumn("dbo.TradeOrders", "IsFlip");
            DropColumn("dbo.AccountBalanceJourneys", "TradeSetId");
        }
    }
}
