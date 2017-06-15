namespace Screen2.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class updatetradereview211020168 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.TradeReviews", "EntryTiming", c => c.Int());
            AddColumn("dbo.TradeReviews", "IsStopTriggered", c => c.Boolean());
            AddColumn("dbo.TradeReviews", "ExitTiming", c => c.Int());
            DropColumn("dbo.TradeReviews", "EntryTimming");
            DropColumn("dbo.TradeReviews", "IsStoppTriggered");
            DropColumn("dbo.TradeReviews", "ExitTimming");
        }
        
        public override void Down()
        {
            AddColumn("dbo.TradeReviews", "ExitTimming", c => c.Int());
            AddColumn("dbo.TradeReviews", "IsStoppTriggered", c => c.Boolean());
            AddColumn("dbo.TradeReviews", "EntryTimming", c => c.Int());
            DropColumn("dbo.TradeReviews", "ExitTiming");
            DropColumn("dbo.TradeReviews", "IsStopTriggered");
            DropColumn("dbo.TradeReviews", "EntryTiming");
        }
    }
}
