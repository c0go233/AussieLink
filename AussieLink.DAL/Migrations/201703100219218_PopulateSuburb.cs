namespace AussieLink.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class PopulateSuburb : DbMigration
    {
        public override void Up()
        {
            Sql("INSERT INTO dbo.Suburbs (StateId, Name) VALUES (1, 'Acton')");
        }
        
        public override void Down()
        {
        }
    }
}
