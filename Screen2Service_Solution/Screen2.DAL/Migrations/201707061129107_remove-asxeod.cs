namespace Screen2.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class removeasxeod : DbMigration
    {
        public override void Up()
        {
            DropIndex("dbo.AsxEods", new[] { "Symbol" });
            DropIndex("dbo.AsxEods", new[] { "TradingDate" });
            DropTable("dbo.AsxEods");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.AsxEods",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Symbol = c.String(nullable: false, maxLength: 8000, unicode: false),
                        TradingDate = c.Int(nullable: false),
                        Open = c.Double(nullable: false),
                        Close = c.Double(nullable: false),
                        High = c.Double(nullable: false),
                        Low = c.Double(nullable: false),
                        Volumn = c.Long(nullable: false),
                        AdjustedClose = c.Double(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateIndex("dbo.AsxEods", "TradingDate");
            CreateIndex("dbo.AsxEods", "Symbol");
        }
    }
}
