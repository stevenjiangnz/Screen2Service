namespace Screen2.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class updatetradereview2110201613 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.TradeSimulateOrders",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        SetRef = c.String(),
                        ShareId = c.Int(nullable: false),
                        EntryTradingDate = c.Int(nullable: false),
                        EntryPrice = c.Double(nullable: false),
                        ExitTradingDate = c.Int(nullable: false),
                        ExitPrice = c.Double(nullable: false),
                        Days = c.Int(nullable: false),
                        Flag = c.Int(nullable: false),
                        Diff_Per = c.Double(nullable: false),
                        UpdatedDT = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.TradeSimulateOrders");
        }
    }
}
