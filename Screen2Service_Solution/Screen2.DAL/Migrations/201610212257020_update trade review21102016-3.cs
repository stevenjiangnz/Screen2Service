namespace Screen2.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class updatetradereview211020163 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.TradeReviews", "IsStoppTriggered", c => c.Boolean());
            AddColumn("dbo.TradeReviews", "IsLimitTriggered", c => c.Boolean());
            DropColumn("dbo.TradeReviews", "IsStopped");
        }
        
        public override void Down()
        {
            AddColumn("dbo.TradeReviews", "IsStopped", c => c.Boolean());
            DropColumn("dbo.TradeReviews", "IsLimitTriggered");
            DropColumn("dbo.TradeReviews", "IsStoppTriggered");
        }
    }
}
