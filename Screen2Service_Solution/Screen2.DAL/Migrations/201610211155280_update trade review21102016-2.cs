namespace Screen2.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class updatetradereview211020162 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.TradeReviews", "UpdatedBy", c => c.String());
            AddColumn("dbo.TradeReviews", "UpdatedDT", c => c.DateTime(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.TradeReviews", "UpdatedDT");
            DropColumn("dbo.TradeReviews", "UpdatedBy");
        }
    }
}
