namespace Screen2.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addStatscanset031120165 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.StatScanSets", "CompleteDt", c => c.DateTime());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.StatScanSets", "CompleteDt", c => c.DateTime(nullable: false));
        }
    }
}
