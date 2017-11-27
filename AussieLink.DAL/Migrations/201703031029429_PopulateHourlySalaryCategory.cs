namespace AussieLink.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class PopulateHourlySalaryCategory : DbMigration
    {
        public override void Up()
        {
            Sql("INSERT INTO dbo.HourlySalaryCategories(HourlySalaryCategoryId, Name, Amount) VALUES(1, '$13.00+', 13)");
            Sql("INSERT INTO dbo.HourlySalaryCategories(HourlySalaryCategoryId, Name, Amount) VALUES(2, '$14.00+', 14)");
            Sql("INSERT INTO dbo.HourlySalaryCategories(HourlySalaryCategoryId, Name, Amount) VALUES(3, '$15.00+', 15)");
            Sql("INSERT INTO dbo.HourlySalaryCategories(HourlySalaryCategoryId, Name, Amount) VALUES(4, '$16.00+', 16)");
            Sql("INSERT INTO dbo.HourlySalaryCategories(HourlySalaryCategoryId, Name, Amount) VALUES(5, '$17.00+', 17)");
            Sql("INSERT INTO dbo.HourlySalaryCategories(HourlySalaryCategoryId, Name, Amount) VALUES(6, '$18.00+', 18)");
            Sql("INSERT INTO dbo.HourlySalaryCategories(HourlySalaryCategoryId, Name, Amount) VALUES(7, '$19.00+', 19)");
            Sql("INSERT INTO dbo.HourlySalaryCategories(HourlySalaryCategoryId, Name, Amount) VALUES(8, '$20.00+', 20)");

        }

        public override void Down()
        {
        }
    }
}
