namespace Screen2.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class removeqsxeod : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.AsxEods", "ShareId", "dbo.Shares");
            DropIndex("dbo.AsxEods", "TradingDate_Index");
            DropIndex("dbo.AsxEods", new[] { "ShareId" });
            DropTable("dbo.AsxEods");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.AsxEods",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        TradingDate = c.Int(nullable: false),
                        Open = c.Double(nullable: false),
                        Close = c.Double(nullable: false),
                        High = c.Double(nullable: false),
                        Low = c.Double(nullable: false),
                        Volumn = c.Long(nullable: false),
                        AdjustedClose = c.Double(),
                        JSTicks = c.Long(nullable: false),
                        ShareId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateIndex("dbo.AsxEods", "ShareId");
            CreateIndex("dbo.AsxEods", "TradingDate", name: "TradingDate_Index");
            AddForeignKey("dbo.AsxEods", "ShareId", "dbo.Shares", "Id", cascadeDelete: true);
        }
    }
}
