namespace AussieLink.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddSharePost : DbMigration
    {
        public override void Up()
        {
            DropPrimaryKey("dbo.Addresses");
            CreateTable(
                "dbo.SharePosts",
                c => new
                    {
                        PostId = c.Int(nullable: false, identity: true),
                        Price = c.Decimal(nullable: false, precision: 18, scale: 2),
                        AvailableFrom = c.DateTime(nullable: false),
                        ShareTypeId = c.Byte(nullable: false),
                        GenderId = c.Byte(nullable: false),
                        UserId = c.Guid(nullable: false),
                        PlaceId = c.Int(nullable: false),
                        Title = c.String(nullable: false),
                        Description = c.String(nullable: false),
                        DateCreated = c.DateTime(nullable: false),
                        Complete = c.Boolean(nullable: false),
                        Cancel = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.PostId)
                .ForeignKey("dbo.Genders", t => t.GenderId, cascadeDelete: false)
                .ForeignKey("dbo.Places", t => t.PlaceId, cascadeDelete: false)
                .ForeignKey("dbo.ShareTypes", t => t.ShareTypeId, cascadeDelete: false)
                .ForeignKey("dbo.Users", t => t.UserId, cascadeDelete: false)
                .Index(t => t.ShareTypeId)
                .Index(t => t.GenderId)
                .Index(t => t.UserId)
                .Index(t => t.PlaceId);
            
            AddColumn("dbo.Addresses", "PostId", c => c.Int(nullable: false));
            AddPrimaryKey("dbo.Addresses", "PostId");
            CreateIndex("dbo.Addresses", "PostId");
            AddForeignKey("dbo.Addresses", "PostId", "dbo.SharePosts", "PostId");
            AddForeignKey("dbo.Comments", "PostId", "dbo.SharePosts", "PostId");
            DropColumn("dbo.Addresses", "AddressId");

            Sql("DBCC CHECKIDENT ('dbo.Comments', RESEED, 1);");
            Sql("DBCC CHECKIDENT ('dbo.SharePosts', RESEED, 1);");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Addresses", "AddressId", c => c.Int(nullable: false, identity: true));
            DropForeignKey("dbo.SharePosts", "UserId", "dbo.Users");
            DropForeignKey("dbo.SharePosts", "ShareTypeId", "dbo.ShareTypes");
            DropForeignKey("dbo.SharePosts", "PlaceId", "dbo.Places");
            DropForeignKey("dbo.SharePosts", "GenderId", "dbo.Genders");
            DropForeignKey("dbo.Comments", "PostId", "dbo.SharePosts");
            DropForeignKey("dbo.Addresses", "PostId", "dbo.SharePosts");
            DropIndex("dbo.SharePosts", new[] { "PlaceId" });
            DropIndex("dbo.SharePosts", new[] { "UserId" });
            DropIndex("dbo.SharePosts", new[] { "GenderId" });
            DropIndex("dbo.SharePosts", new[] { "ShareTypeId" });
            DropIndex("dbo.Addresses", new[] { "PostId" });
            DropPrimaryKey("dbo.Addresses");
            DropColumn("dbo.Addresses", "PostId");
            DropTable("dbo.SharePosts");
            AddPrimaryKey("dbo.Addresses", "AddressId");
        }
    }
}
