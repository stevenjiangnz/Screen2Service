namespace Screen2.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class updateTradeOrder13102016 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.TradePositions", "LastProcessedDate", c => c.Int());
        }
        
        public override void Down()
        {
            DropColumn("dbo.TradePositions", "LastProcessedDate");
        }
    }
}
