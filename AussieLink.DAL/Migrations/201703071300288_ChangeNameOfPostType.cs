namespace AussieLink.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChangeNameOfPostType : DbMigration
    {
        public override void Up()
        {
            Sql("UPDATE dbo.PostTypes SET Name='Job' WHERE PostTypeId=1");
            Sql("UPDATE dbo.PostTypes SET Name='Rent' WHERE PostTypeId=2");
            Sql("UPDATE dbo.PostTypes SET Name='Second Hand' WHERE PostTypeId=3");
        }
        
        public override void Down()
        {
        }
    }
}
