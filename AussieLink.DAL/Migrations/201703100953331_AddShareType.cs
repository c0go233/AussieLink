namespace AussieLink.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddShareType : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ShareTypes",
                c => new
                    {
                        ShareTypeId = c.Byte(nullable: false),
                        Name = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.ShareTypeId);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.ShareTypes");
        }
    }
}
