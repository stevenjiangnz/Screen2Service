namespace Screen2.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class updateRecord0711201603 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Records", "ZoneId", c => c.Int());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Records", "ZoneId", c => c.Int(nullable: false));
        }
    }
}
