namespace Screen2.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class updateRule24102016 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Rules", "Direction", c => c.String());
            AddColumn("dbo.Rules", "Assembly", c => c.String());
            AddColumn("dbo.Rules", "UpdatedDT", c => c.DateTime(nullable: false));
            AlterColumn("dbo.Rules", "Type", c => c.String());
            AlterColumn("dbo.Rules", "Formula", c => c.String());
            AlterColumn("dbo.Rules", "Owner", c => c.String());
            DropColumn("dbo.Rules", "Category");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Rules", "Category", c => c.String());
            AlterColumn("dbo.Rules", "Owner", c => c.String(nullable: false));
            AlterColumn("dbo.Rules", "Formula", c => c.String(nullable: false));
            AlterColumn("dbo.Rules", "Type", c => c.String(nullable: false));
            DropColumn("dbo.Rules", "UpdatedDT");
            DropColumn("dbo.Rules", "Assembly");
            DropColumn("dbo.Rules", "Direction");
        }
    }
}
