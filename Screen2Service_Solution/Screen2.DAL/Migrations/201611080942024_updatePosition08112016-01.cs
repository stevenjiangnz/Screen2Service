namespace Screen2.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class updatePosition0811201601 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.TradePositions", "Margin", c => c.Double(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.TradePositions", "Margin");
        }
    }
}
