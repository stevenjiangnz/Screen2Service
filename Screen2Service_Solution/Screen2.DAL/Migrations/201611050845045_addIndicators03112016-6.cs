namespace Screen2.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addIndicators031120166 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Indicators", "RSI2", c => c.Double());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Indicators", "RSI2");
        }
    }
}
