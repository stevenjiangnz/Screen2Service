namespace Screen2.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class test : DbMigration
    {
        public override void Up()
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
                .PrimaryKey(t => t.Id)
                .Index(t => t.Symbol)
                .Index(t => t.TradingDate);
            
            DropColumn("dbo.Zones", "IsCurrent");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Zones", "IsCurrent", c => c.Boolean(nullable: false));
            DropIndex("dbo.AsxEods", new[] { "TradingDate" });
            DropIndex("dbo.AsxEods", new[] { "Symbol" });
            DropTable("dbo.AsxEods");
        }
    }
}
