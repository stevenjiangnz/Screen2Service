namespace Screen2.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addasxeod : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.AsxEods",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Symbol = c.String(),
                        TradingDate = c.Int(nullable: false),
                        Open = c.Double(nullable: false),
                        Close = c.Double(nullable: false),
                        High = c.Double(nullable: false),
                        Low = c.Double(nullable: false),
                        Volumn = c.Long(nullable: false),
                        AdjustedClose = c.Double(),
                        ShareId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Shares", t => t.ShareId, cascadeDelete: true)
                .Index(t => t.ShareId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.AsxEods", "ShareId", "dbo.Shares");
            DropIndex("dbo.AsxEods", new[] { "ShareId" });
            DropTable("dbo.AsxEods");
        }
    }
}
