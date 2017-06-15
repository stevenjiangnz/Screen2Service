namespace Screen2.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class updaterecord12112016 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Records", "OriginalFileName", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Records", "OriginalFileName");
        }
    }
}
