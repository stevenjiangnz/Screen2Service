namespace Screen2.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class updateZone : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Zones", "IsCurrent", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Zones", "IsCurrent");
        }
    }
}
