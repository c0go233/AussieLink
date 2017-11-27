namespace AussieLink.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddPicture : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Pictures",
                c => new
                    {
                        PictureId = c.Int(nullable: false, identity: true),
                        PostId = c.Int(nullable: false),
                        Data = c.Binary(),
                        ImageName = c.String(),
                        Size = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.PictureId)
                .ForeignKey("dbo.SharePosts", t => t.PostId, cascadeDelete: true)
                .Index(t => t.PostId);

            Sql("DBCC CHECKIDENT ('dbo.Pictures', RESEED, 1);");

        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Pictures", "PostId", "dbo.SharePosts");
            DropIndex("dbo.Pictures", new[] { "PostId" });
            DropTable("dbo.Pictures");
        }
    }
}
