namespace AussieLink.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class PopulateGender : DbMigration
    {
        public override void Up()
        {
            Sql("INSERT INTO dbo.Genders (GenderId, Name) VALUES (1, 'No Gender Preference')");
            Sql("INSERT INTO dbo.Genders (GenderId, Name) VALUES (2, 'Female Preferred')");
            Sql("INSERT INTO dbo.Genders (GenderId, Name) VALUES (3, 'Male Preferred')");
        }

        public override void Down()
        {
        }
    }
}
