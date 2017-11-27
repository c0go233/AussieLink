namespace AussieLink.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class PopulateOthersForSuburbs : DbMigration
    {
        public override void Up()
        {
            Sql("INSERT INTO dbo.Suburbs (PlaceId, Name) VALUES (1, 'Others')");
            Sql("INSERT INTO dbo.Suburbs (PlaceId, Name) VALUES (2, 'Others')");
            Sql("INSERT INTO dbo.Suburbs (PlaceId, Name) VALUES (3, 'Others')");
            Sql("INSERT INTO dbo.Suburbs (PlaceId, Name) VALUES (4, 'Others')");
            Sql("INSERT INTO dbo.Suburbs (PlaceId, Name) VALUES (5, 'Others')");

        }
        
        public override void Down()
        {
        }
    }
}
