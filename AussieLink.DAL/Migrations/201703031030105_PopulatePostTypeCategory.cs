namespace AussieLink.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class PopulatePostTypeCategory : DbMigration
    {
        public override void Up()
        {
            Sql("INSERT INTO dbo.PostTypes (PostTypeId, Name) VALUES (1, 'JobPost')");
            Sql("INSERT INTO dbo.PostTypes (PostTypeId, Name) VALUES (2, 'RentPost')");
            Sql("INSERT INTO dbo.PostTypes (PostTypeId, Name) VALUES (3, 'SecondhandPost')");
        }

        public override void Down()
        {
        }
    }
}
