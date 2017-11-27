namespace AussieLink.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class PopulateDayCategory : DbMigration
    {
        public override void Up()
        {
            Sql("INSERT INTO dbo.DayCategories (DayCategoryId, Name, Amount) VALUES (1,'1 Day',1)");
            Sql("INSERT INTO dbo.DayCategories (DayCategoryId, Name, Amount) VALUES (2,'2 Days',2)");
            Sql("INSERT INTO dbo.DayCategories (DayCategoryId, Name, Amount) VALUES (3,'3 Days',3)");
            Sql("INSERT INTO dbo.DayCategories (DayCategoryId, Name, Amount) VALUES (4,'4 Days',4)");
            Sql("INSERT INTO dbo.DayCategories (DayCategoryId, Name, Amount) VALUES (5,'5 Days',5)");
            Sql("INSERT INTO dbo.DayCategories (DayCategoryId, Name, Amount) VALUES (6,'6 Days',6)");
            Sql("INSERT INTO dbo.DayCategories (DayCategoryId, Name, Amount) VALUES (7,'7 Days',7)");
        }

        public override void Down()
        {
        }
    }
}
