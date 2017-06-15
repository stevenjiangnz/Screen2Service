namespace Screen2.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class updatetradereview2110201611 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.TradeReviews", "TrendRating", c => c.Int());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.TradeReviews", "TrendRating", c => c.Boolean());
        }
    }
}
