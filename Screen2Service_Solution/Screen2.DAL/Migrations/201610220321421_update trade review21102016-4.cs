namespace Screen2.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class updatetradereview211020164 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.TradeReviews", "EntryPercent", c => c.Double());
            AddColumn("dbo.TradeReviews", "ExitPercent", c => c.Double());
        }
        
        public override void Down()
        {
            DropColumn("dbo.TradeReviews", "ExitPercent");
            DropColumn("dbo.TradeReviews", "EntryPercent");
        }
    }
}
