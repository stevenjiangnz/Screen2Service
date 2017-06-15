namespace Screen2.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class updateRecord0711201601 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Records", "TradingDate", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Records", "TradingDate");
        }
    }
}
