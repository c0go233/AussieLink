namespace AussieLink.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class PopulateJobTypeCategory : DbMigration
    {
        public override void Up()
        {
            Sql("INSERT INTO dbo.JobTypes (Name) VALUES ('Accounting')");
            Sql("INSERT INTO dbo.JobTypes (Name) VALUES ('Administration & Office Support')");
            Sql("INSERT INTO dbo.JobTypes (Name) VALUES ('Art & Media')");
            Sql("INSERT INTO dbo.JobTypes (Name) VALUES ('Construction')");
            Sql("INSERT INTO dbo.JobTypes (Name) VALUES ('Banking & Financial Services')");
            Sql("INSERT INTO dbo.JobTypes (Name) VALUES ('Customer Service & Call Centre')");
            Sql("INSERT INTO dbo.JobTypes (Name) VALUES ('Architecture')");
            Sql("INSERT INTO dbo.JobTypes (Name) VALUES ('Education')");
            Sql("INSERT INTO dbo.JobTypes (Name) VALUES ('Engineering')");
            Sql("INSERT INTO dbo.JobTypes (Name) VALUES ('Farming')");
            Sql("INSERT INTO dbo.JobTypes (Name) VALUES ('Heathcare & Nursing')");
            Sql("INSERT INTO dbo.JobTypes (Name) VALUES ('IT')");
            Sql("INSERT INTO dbo.JobTypes (Name) VALUES ('Hospitality')");
            Sql("INSERT INTO dbo.JobTypes (Name) VALUES ('Marketing')");
            Sql("INSERT INTO dbo.JobTypes (Name) VALUES ('Retail')");
            Sql("INSERT INTO dbo.JobTypes (Name) VALUES ('Legal')");
            Sql("INSERT INTO dbo.JobTypes (Name) VALUES ('Cooking')");
            Sql("INSERT INTO dbo.JobTypes (Name) VALUES ('Other Jobs')");
        }

        public override void Down()
        {
        }
    }
}
