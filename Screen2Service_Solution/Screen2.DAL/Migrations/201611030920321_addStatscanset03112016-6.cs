namespace Screen2.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addStatscanset031120166 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.StatScanSets", "WatchId", c => c.Int(nullable: false));
            AddColumn("dbo.StatScanSets", "StartDate", c => c.Int(nullable: false));
            AddColumn("dbo.StatScanSets", "EndDate", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.StatScanSets", "EndDate");
            DropColumn("dbo.StatScanSets", "StartDate");
            DropColumn("dbo.StatScanSets", "WatchId");
        }
    }
}
