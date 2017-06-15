namespace Screen2.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addtradereview : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.TradeReviews",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        EntryTimming = c.Int(),
                        IsTrend = c.Boolean(),
                        IsLong = c.Boolean(),
                        IsDirectionCorrect = c.Boolean(),
                        IsProfitable = c.Boolean(),
                        IsStopped = c.Boolean(),
                        ExitTimming = c.Boolean(),
                        Notes = c.String(),
                        TradePositionId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.TradePositions", t => t.TradePositionId, cascadeDelete: true)
                .Index(t => t.TradePositionId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.TradeReviews", "TradePositionId", "dbo.TradePositions");
            DropIndex("dbo.TradeReviews", new[] { "TradePositionId" });
            DropTable("dbo.TradeReviews");
        }
    }
}
