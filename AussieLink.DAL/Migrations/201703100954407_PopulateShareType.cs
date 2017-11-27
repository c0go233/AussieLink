namespace AussieLink.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class PopulateShareType : DbMigration
    {
        public override void Up()
        {
            Sql("INSERT INTO dbo.ShareTypes (ShareTypeId, Name) VALUES (1, 'Single Room')");
            Sql("INSERT INTO dbo.ShareTypes (ShareTypeId, Name) VALUES (2, 'Double Room')");
            Sql("INSERT INTO dbo.ShareTypes (ShareTypeId, Name) VALUES (3, 'Triple Room')");
            Sql("INSERT INTO dbo.ShareTypes (ShareTypeId, Name) VALUES (4, 'Quadruple Room')");
            Sql("INSERT INTO dbo.ShareTypes (ShareTypeId, Name) VALUES (5, 'Quintuple Room')");
            Sql("INSERT INTO dbo.ShareTypes (ShareTypeId, Name) VALUES (6, 'hexatruple Room')");
            Sql("INSERT INTO dbo.ShareTypes (ShareTypeId, Name) VALUES (7, 'Others')");

        }
        
        public override void Down()
        {
        }
    }
}
