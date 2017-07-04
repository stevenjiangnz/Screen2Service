namespace Screen2.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class updateasxeod : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AsxEods", "JSTicks", c => c.Long(nullable: false));
            CreateIndex("dbo.AsxEods", "TradingDate", name: "TradingDate_Index");
            DropColumn("dbo.AsxEods", "Symbol");
        }
        
        public override void Down()
        {
            AddColumn("dbo.AsxEods", "Symbol", c => c.String());
            DropIndex("dbo.AsxEods", "TradingDate_Index");
            DropColumn("dbo.AsxEods", "JSTicks");
        }
    }
}
