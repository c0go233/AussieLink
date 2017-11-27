namespace AussieLink.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddAddress : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Addresses",
                c => new
                    {
                        AddressId = c.Int(nullable: false, identity: true),
                        PlaceId = c.Int(nullable: false),
                        SuburbId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.AddressId)
                .ForeignKey("dbo.Places", t => t.PlaceId, cascadeDelete: false)
                .ForeignKey("dbo.Suburbs", t => t.SuburbId, cascadeDelete: false)
                .Index(t => t.PlaceId)
                .Index(t => t.SuburbId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Addresses", "SuburbId", "dbo.Suburbs");
            DropForeignKey("dbo.Addresses", "PlaceId", "dbo.Places");
            DropIndex("dbo.Addresses", new[] { "SuburbId" });
            DropIndex("dbo.Addresses", new[] { "PlaceId" });
            DropTable("dbo.Addresses");
        }
    }
}
