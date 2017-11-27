namespace AussieLink.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class PopulateState : DbMigration
    {
        public override void Up()
        {
            Sql("INSERT INTO dbo.States (StateId, Name) VALUES (1, 'ACT')");
            Sql("INSERT INTO dbo.States (StateId, Name) VALUES (2, 'NSW')");
            Sql("INSERT INTO dbo.States (StateId, Name) VALUES (3, 'NT')");
            Sql("INSERT INTO dbo.States (StateId, Name) VALUES (4, 'QLD')");
            Sql("INSERT INTO dbo.States (StateId, Name) VALUES (5, 'SA')");
            Sql("INSERT INTO dbo.States (StateId, Name) VALUES (6, 'TAS')");
            Sql("INSERT INTO dbo.States (StateId, Name) VALUES (7, 'VIC')");
            Sql("INSERT INTO dbo.States (StateId, Name) VALUES (8, 'WA')");
        }
        
        public override void Down()
        {
        }
    }
}
