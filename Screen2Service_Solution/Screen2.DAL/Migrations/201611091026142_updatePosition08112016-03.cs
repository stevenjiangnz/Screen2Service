namespace Screen2.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class updatePosition0811201603 : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.AccountBalances", "TotalBalance");
        }
        
        public override void Down()
        {
            AddColumn("dbo.AccountBalances", "TotalBalance", c => c.Double(nullable: false));
        }
    }
}
