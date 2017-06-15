namespace Screen2.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class updatetradereview211020166 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.TradeReviews", "ProfitRating", c => c.Int());
            DropColumn("dbo.TradeReviews", "IsProfit");
        }
        
        public override void Down()
        {
            AddColumn("dbo.TradeReviews", "IsProfit", c => c.Boolean());
            DropColumn("dbo.TradeReviews", "ProfitRating");
        }
    }
}
