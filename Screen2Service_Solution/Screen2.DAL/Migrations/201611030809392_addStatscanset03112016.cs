namespace Screen2.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addStatscanset03112016 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.StatScanSets",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        SetRef = c.String(),
                        EntryLogic = c.String(),
                        Notes = c.String(),
                        StartDt = c.DateTime(nullable: false),
                        CompleteDt = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.StatScanSets");
        }
    }
}
