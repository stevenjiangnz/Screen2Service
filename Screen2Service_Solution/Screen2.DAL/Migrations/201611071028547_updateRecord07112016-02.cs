namespace Screen2.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class updateRecord0711201602 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Records", "Owner", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Records", "Owner");
        }
    }
}
