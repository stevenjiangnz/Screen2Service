namespace Screen2.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class updatetradereview2110201610 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.TradeReviews", "IsExitBBTriggered", c => c.Boolean());
            DropColumn("dbo.TradeReviews", "IsExitBBTrigger");
        }
        
        public override void Down()
        {
            AddColumn("dbo.TradeReviews", "IsExitBBTrigger", c => c.Boolean());
            DropColumn("dbo.TradeReviews", "IsExitBBTriggered");
        }
    }
}
