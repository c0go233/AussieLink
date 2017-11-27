namespace AussieLink.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddPictureTypeToPicture : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Pictures", "PictureType", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Pictures", "PictureType");
        }
    }
}
