namespace Screen2.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class updatetradereview2110201612 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.TradeReviews", "IsAddSize", c => c.Boolean());
        }
        
        public override void Down()
        {
            DropColumn("dbo.TradeReviews", "IsAddSize");
        }
    }
}
