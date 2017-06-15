namespace Screen2.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class updatetransaction20102016 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.TradePositions", "days", c => c.Int());
        }
        
        public override void Down()
        {
            DropColumn("dbo.TradePositions", "days");
        }
    }
}
