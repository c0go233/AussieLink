namespace AussieLink.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class PopulateWeeklySalaryCategory : DbMigration
    {
        public override void Up()
        {
            Sql("INSERT INTO dbo.WeeklySalaryCategories(WeeklySalaryCategoryId, Name, Amount) VALUES(1, '$300.00+', 300)");
            Sql("INSERT INTO dbo.WeeklySalaryCategories(WeeklySalaryCategoryId, Name, Amount) VALUES(2, '$400.00+', 400)");
            Sql("INSERT INTO dbo.WeeklySalaryCategories(WeeklySalaryCategoryId, Name, Amount) VALUES(3, '$500.00+', 500)");
            Sql("INSERT INTO dbo.WeeklySalaryCategories(WeeklySalaryCategoryId, Name, Amount) VALUES(4,'$600.00+', 600)");
            Sql("INSERT INTO dbo.WeeklySalaryCategories(WeeklySalaryCategoryId, Name, Amount) VALUES(5, '$700.00+', 700)");
            Sql("INSERT INTO dbo.WeeklySalaryCategories(WeeklySalaryCategoryId, Name, Amount) VALUES(6, '$800.00+', 800)");
            Sql("INSERT INTO dbo.WeeklySalaryCategories(WeeklySalaryCategoryId, Name, Amount) VALUES(7, '$900.00+', 900)");
            Sql("INSERT INTO dbo.WeeklySalaryCategories(WeeklySalaryCategoryId, Name, Amount) VALUES(8, '$1000.00+', 1000)");
        }

        public override void Down()
        {
        }
    }
}
