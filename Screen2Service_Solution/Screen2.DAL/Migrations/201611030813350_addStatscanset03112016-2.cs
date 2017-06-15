namespace Screen2.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addStatscanset031120162 : DbMigration
    {
        public override void Up()
        {
            DropTable("dbo.TradeSetReviews");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.TradeSetReviews",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        content = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
        }
    }
}
