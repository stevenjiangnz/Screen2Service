namespace Screen2.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class updatetradereview21102016 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.TradeReviews", "IsProfit", c => c.Boolean());
            AddColumn("dbo.TradeReviews", "IsEntryLong", c => c.Boolean());
            AddColumn("dbo.TradeReviews", "IsExitBBTrigger", c => c.Boolean());
            AddColumn("dbo.TradeReviews", "BBEntryPercent", c => c.Double());
            AddColumn("dbo.TradeReviews", "BBExitPercent", c => c.Double());
            AddColumn("dbo.TradeReviews", "Volatility", c => c.Int());
            AddColumn("dbo.TradeReviews", "DaysSpan", c => c.Int());
            AddColumn("dbo.TradeReviews", "Diff", c => c.Double());
            AddColumn("dbo.TradeReviews", "Diff_Per", c => c.Double());
            DropColumn("dbo.TradeReviews", "IsLong");
            DropColumn("dbo.TradeReviews", "IsProfitable");
        }
        
        public override void Down()
        {
            AddColumn("dbo.TradeReviews", "IsProfitable", c => c.Boolean());
            AddColumn("dbo.TradeReviews", "IsLong", c => c.Boolean());
            DropColumn("dbo.TradeReviews", "Diff_Per");
            DropColumn("dbo.TradeReviews", "Diff");
            DropColumn("dbo.TradeReviews", "DaysSpan");
            DropColumn("dbo.TradeReviews", "Volatility");
            DropColumn("dbo.TradeReviews", "BBExitPercent");
            DropColumn("dbo.TradeReviews", "BBEntryPercent");
            DropColumn("dbo.TradeReviews", "IsExitBBTrigger");
            DropColumn("dbo.TradeReviews", "IsEntryLong");
            DropColumn("dbo.TradeReviews", "IsProfit");
        }
    }
}
