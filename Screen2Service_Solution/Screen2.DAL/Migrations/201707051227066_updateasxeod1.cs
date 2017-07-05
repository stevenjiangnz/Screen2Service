namespace Screen2.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class updateasxeod1 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.AsxEods", "ShareId", "dbo.Shares");
            DropIndex("dbo.AsxEods", new[] { "ShareId" });
            RenameIndex(table: "dbo.AsxEods", name: "TradingDate_Index", newName: "IX_TradingDate");
            AddColumn("dbo.AsxEods", "Symbol", c => c.String(nullable: false, maxLength: 8000, unicode: false));
            CreateIndex("dbo.AsxEods", "Symbol");
            DropColumn("dbo.AsxEods", "JSTicks");
            DropColumn("dbo.AsxEods", "ShareId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.AsxEods", "ShareId", c => c.Int(nullable: false));
            AddColumn("dbo.AsxEods", "JSTicks", c => c.Long(nullable: false));
            DropIndex("dbo.AsxEods", new[] { "Symbol" });
            DropColumn("dbo.AsxEods", "Symbol");
            RenameIndex(table: "dbo.AsxEods", name: "IX_TradingDate", newName: "TradingDate_Index");
            CreateIndex("dbo.AsxEods", "ShareId");
            AddForeignKey("dbo.AsxEods", "ShareId", "dbo.Shares", "Id", cascadeDelete: true);
        }
    }
}
