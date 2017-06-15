namespace Screen2.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class updateTradeOrder111020164 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.TradePositions", "EntryFee", c => c.Double(nullable: false));
            AddColumn("dbo.TradePositions", "ExistFee", c => c.Double(nullable: false));
            AddColumn("dbo.TradePositions", "ExistPrice", c => c.Double(nullable: false));
            AddColumn("dbo.TradePositions", "EntryTransactionId", c => c.Int(nullable: false));
            AddColumn("dbo.TradePositions", "ExitTransactionId", c => c.Int());
            DropColumn("dbo.TradePositions", "Fee");
            DropColumn("dbo.TradePositions", "CurrentValue");
        }
        
        public override void Down()
        {
            AddColumn("dbo.TradePositions", "CurrentValue", c => c.Double(nullable: false));
            AddColumn("dbo.TradePositions", "Fee", c => c.Double(nullable: false));
            DropColumn("dbo.TradePositions", "ExitTransactionId");
            DropColumn("dbo.TradePositions", "EntryTransactionId");
            DropColumn("dbo.TradePositions", "ExistPrice");
            DropColumn("dbo.TradePositions", "ExistFee");
            DropColumn("dbo.TradePositions", "EntryFee");
        }
    }
}
