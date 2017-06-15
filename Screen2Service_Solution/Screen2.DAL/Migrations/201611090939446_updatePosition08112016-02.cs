namespace Screen2.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class updatePosition0811201602 : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.AccountBalances", "Margin");
            DropColumn("dbo.AccountBalances", "Reserve");
            DropColumn("dbo.AccountBalances", "PositionValue");
        }
        
        public override void Down()
        {
            AddColumn("dbo.AccountBalances", "PositionValue", c => c.Double(nullable: false));
            AddColumn("dbo.AccountBalances", "Reserve", c => c.Double(nullable: false));
            AddColumn("dbo.AccountBalances", "Margin", c => c.Double(nullable: false));
        }
    }
}
