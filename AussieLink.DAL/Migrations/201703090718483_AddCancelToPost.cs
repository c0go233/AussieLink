namespace AussieLink.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddCancelToPost : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.JobPosts", "Cancel", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.JobPosts", "Cancel");
        }
    }
}
