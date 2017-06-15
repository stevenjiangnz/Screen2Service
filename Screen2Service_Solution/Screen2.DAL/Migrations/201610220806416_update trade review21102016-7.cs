namespace Screen2.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class updatetradereview211020167 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.TradeReviews", "OverallRating", c => c.Int());
        }
        
        public override void Down()
        {
            DropColumn("dbo.TradeReviews", "OverallRating");
        }
    }
}
