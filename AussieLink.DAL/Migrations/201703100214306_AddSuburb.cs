namespace AussieLink.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddSuburb : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Suburbs",
                c => new
                    {
                        SuburbId = c.Int(nullable: false, identity: true),
                        StateId = c.Byte(nullable: false),
                        Name = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.SuburbId)
                .ForeignKey("dbo.States", t => t.StateId, cascadeDelete: true)
                .Index(t => t.StateId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Suburbs", "StateId", "dbo.States");
            DropIndex("dbo.Suburbs", new[] { "StateId" });
            DropTable("dbo.Suburbs");
        }
    }
}
