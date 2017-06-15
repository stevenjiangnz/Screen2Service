namespace Screen2.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class TrachingAccountUpdate : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Accounts", "IsTrackingAccount", c => c.Boolean(nullable: false));
            AddColumn("dbo.TradePositions", "OtherCost", c => c.Double());
        }
        
        public override void Down()
        {
            DropColumn("dbo.TradePositions", "OtherCost");
            DropColumn("dbo.Accounts", "IsTrackingAccount");
        }
    }
}
