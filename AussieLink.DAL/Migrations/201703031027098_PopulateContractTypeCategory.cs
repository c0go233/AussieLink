namespace AussieLink.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class PopulateContractTypeCategory : DbMigration
    {
        public override void Up()
        {
            Sql("INSERT INTO dbo.ContractTypes (Name) VALUES ('Full-Time')");
            Sql("INSERT INTO dbo.ContractTypes (Name) VALUES ('Part-Time')");
            Sql("INSERT INTO dbo.ContractTypes (Name) VALUES ('Internship')");
            Sql("INSERT INTO dbo.ContractTypes (Name) VALUES ('Temporary')");
        }

        public override void Down()
        {
        }
    }
}
