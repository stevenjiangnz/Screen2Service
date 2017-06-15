namespace Screen2.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class updatetradereview211020165 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.TradeReviews", "ExitTimming", c => c.Int());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.TradeReviews", "ExitTimming", c => c.Boolean());
        }
    }
}
