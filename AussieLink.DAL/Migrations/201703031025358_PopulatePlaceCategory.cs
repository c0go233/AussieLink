namespace AussieLink.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class PopulatePlaceCategory : DbMigration
    {
        public override void Up()
        {
            Sql("INSERT INTO dbo.Places (Name) VALUES ('Sydney')");
            Sql("INSERT INTO dbo.Places (Name) VALUES ('Melbourne')");
            Sql("INSERT INTO dbo.Places (Name) VALUES ('Brisbane')");
            Sql("INSERT INTO dbo.Places (Name) VALUES ('Adelaide')");
            Sql("INSERT INTO dbo.Places (Name) VALUES ('Queensland')");
        }

        public override void Down()
        {
        }
    }
}
