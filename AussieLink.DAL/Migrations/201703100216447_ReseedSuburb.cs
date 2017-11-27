namespace AussieLink.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ReseedSuburb : DbMigration
    {
        public override void Up()
        {
            Sql("DBCC CHECKIDENT ('dbo.Suburbs', RESEED, 1);");
        }
        
        public override void Down()
        {
        }
    }
}
