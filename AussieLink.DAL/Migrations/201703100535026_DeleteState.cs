namespace AussieLink.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DeleteState : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Suburbs", "StateId", "dbo.States");
            DropIndex("dbo.Suburbs", new[] { "StateId" });
            AddColumn("dbo.Suburbs", "PlaceId", c => c.Int(nullable: false));
            CreateIndex("dbo.Suburbs", "PlaceId");
            AddForeignKey("dbo.Suburbs", "PlaceId", "dbo.Places", "PlaceId", cascadeDelete: true);
            DropColumn("dbo.Suburbs", "StateId");
            DropTable("dbo.States");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.States",
                c => new
                    {
                        StateId = c.Byte(nullable: false),
                        Name = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.StateId);
            
            AddColumn("dbo.Suburbs", "StateId", c => c.Byte(nullable: false));
            DropForeignKey("dbo.Suburbs", "PlaceId", "dbo.Places");
            DropIndex("dbo.Suburbs", new[] { "PlaceId" });
            DropColumn("dbo.Suburbs", "PlaceId");
            CreateIndex("dbo.Suburbs", "StateId");
            AddForeignKey("dbo.Suburbs", "StateId", "dbo.States", "StateId", cascadeDelete: true);
        }
    }
}
