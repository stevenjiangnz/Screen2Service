namespace Screen2.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class updatetradereview211020169 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.TradeReviews", "TrendRating", c => c.Boolean());
            AddColumn("dbo.TradeReviews", "IsReverse", c => c.Boolean());
            DropColumn("dbo.TradeReviews", "IsTrend");
        }
        
        public override void Down()
        {
            AddColumn("dbo.TradeReviews", "IsTrend", c => c.Boolean());
            DropColumn("dbo.TradeReviews", "IsReverse");
            DropColumn("dbo.TradeReviews", "TrendRating");
        }
    }
}
