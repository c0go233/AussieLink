namespace AussieLink.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ReseedAddress : DbMigration
    {
        public override void Up()
        {
            Sql("DBCC CHECKIDENT ('dbo.Addresses', RESEED, 1);");
        }
        
        public override void Down()
        {
        }
    }
}
