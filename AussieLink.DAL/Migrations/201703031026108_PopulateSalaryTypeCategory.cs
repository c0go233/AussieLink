namespace AussieLink.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class PopulateSalaryTypeCategory : DbMigration
    {
        public override void Up()
        {
            Sql("INSERT INTO dbo.SalaryTypes (Name) VALUES('A Hour')");
            Sql("INSERT INTO dbo.SalaryTypes (Name) VALUES('A Week')");
        }

        public override void Down()
        {
        }
    }
}
