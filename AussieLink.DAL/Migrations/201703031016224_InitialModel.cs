namespace AussieLink.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialModel : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Comments",
                c => new
                    {
                        CommentId = c.Int(nullable: false, identity: true),
                        UserId = c.Guid(nullable: false),
                        PostId = c.Int(nullable: false),
                        PostTypeId = c.Byte(nullable: false),
                        Description = c.String(nullable: false),
                        DateCreated = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.CommentId)
                .ForeignKey("dbo.Users", t => t.UserId, cascadeDelete: true)
                .ForeignKey("dbo.JobPosts", t => t.PostId)
                .ForeignKey("dbo.PostTypes", t => t.PostTypeId)
                .Index(t => t.UserId)
                .Index(t => t.PostId)
                .Index(t => t.PostTypeId);
            
            CreateTable(
                "dbo.Users",
                c => new
                    {
                        UserId = c.Guid(nullable: false),
                        Name = c.String(nullable: false, maxLength: 50),
                        Email = c.String(nullable: false, maxLength: 100),
                        Password = c.String(),
                        DateCreated = c.DateTime(nullable: false),
                        IsCanceled = c.Boolean(nullable: false),
                        Salt = c.String(),
                        SocialLogin = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.UserId);
            
            CreateTable(
                "dbo.ContractTypes",
                c => new
                    {
                        ContractTypeId = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.ContractTypeId);
            
            CreateTable(
                "dbo.HourlySalaryCategories",
                c => new
                    {
                        HourlySalaryCategoryId = c.Byte(nullable: false),
                        Name = c.String(nullable: false),
                        Amount = c.Decimal(nullable: false, precision: 18, scale: 2),
                    })
                .PrimaryKey(t => t.HourlySalaryCategoryId);
            
            CreateTable(
                "dbo.JobDays",
                c => new
                    {
                        JobDayId = c.Int(nullable: false, identity: true),
                        PostId = c.Int(nullable: false),
                        Amount = c.Byte(nullable: false),
                        Size = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.JobDayId)
                .ForeignKey("dbo.JobPosts", t => t.PostId, cascadeDelete: true)
                .Index(t => t.PostId);
            
            CreateTable(
                "dbo.JobPosts",
                c => new
                    {
                        PostId = c.Int(nullable: false, identity: true),
                        JobTypeId = c.Int(nullable: false),
                        ContractTypeId = c.Int(nullable: false),
                        UserId = c.Guid(nullable: false),
                        PlaceId = c.Int(nullable: false),
                        Title = c.String(nullable: false),
                        Description = c.String(nullable: false),
                        DateCreated = c.DateTime(nullable: false),
                        Complete = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.PostId)
                .ForeignKey("dbo.ContractTypes", t => t.ContractTypeId, cascadeDelete: true)
                .ForeignKey("dbo.JobTypes", t => t.JobTypeId, cascadeDelete: true)
                .ForeignKey("dbo.Places", t => t.PlaceId, cascadeDelete: true)
                .ForeignKey("dbo.Users", t => t.UserId, cascadeDelete: true)
                .Index(t => t.JobTypeId)
                .Index(t => t.ContractTypeId)
                .Index(t => t.UserId)
                .Index(t => t.PlaceId);
            
            CreateTable(
                "dbo.JobTypes",
                c => new
                    {
                        JobTypeId = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.JobTypeId);
            
            CreateTable(
                "dbo.Places",
                c => new
                    {
                        PlaceId = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.PlaceId);
            
            CreateTable(
                "dbo.Salaries",
                c => new
                    {
                        SalaryId = c.Int(nullable: false, identity: true),
                        Amount = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Size = c.String(nullable: false),
                        PostId = c.Int(nullable: false),
                        SalaryTypeId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.SalaryId)
                .ForeignKey("dbo.SalaryTypes", t => t.SalaryTypeId, cascadeDelete: true)
                .ForeignKey("dbo.JobPosts", t => t.PostId, cascadeDelete: true)
                .Index(t => t.PostId)
                .Index(t => t.SalaryTypeId);
            
            CreateTable(
                "dbo.SalaryTypes",
                c => new
                    {
                        SalaryTypeId = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.SalaryTypeId);
            
            CreateTable(
                "dbo.PostTypes",
                c => new
                    {
                        PostTypeId = c.Byte(nullable: false),
                        Name = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.PostTypeId);
            
            CreateTable(
                "dbo.ResetPasswordLinks",
                c => new
                    {
                        ResetPasswordLinkId = c.Int(nullable: false, identity: true),
                        UserId = c.Guid(nullable: false),
                        Clicked = c.Boolean(nullable: false),
                        DateCreated = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.ResetPasswordLinkId)
                .ForeignKey("dbo.Users", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.WeeklySalaryCategories",
                c => new
                    {
                        WeeklySalaryCategoryId = c.Byte(nullable: false),
                        Name = c.String(nullable: false),
                        Amount = c.Decimal(nullable: false, precision: 18, scale: 2),
                    })
                .PrimaryKey(t => t.WeeklySalaryCategoryId);
            
            CreateTable(
                "dbo.DayCategories",
                c => new
                    {
                        DayCategoryId = c.Byte(nullable: false),
                        Name = c.String(nullable: false),
                        Amount = c.Byte(nullable: false),
                    })
                .PrimaryKey(t => t.DayCategoryId);

            Sql("DBCC CHECKIDENT ('dbo.SalaryTypes', RESEED, 1);");
            Sql("DBCC CHECKIDENT ('dbo.JobDays', RESEED, 1);");
            Sql("DBCC CHECKIDENT ('dbo.Salaries', RESEED, 1);");
            Sql("DBCC CHECKIDENT ('dbo.Places', RESEED, 1);");
            Sql("DBCC CHECKIDENT ('dbo.Places', RESEED, 1);");
            Sql("DBCC CHECKIDENT ('dbo.JobPosts', RESEED, 1);");
            Sql("DBCC CHECKIDENT ('dbo.ContractTypes', RESEED, 1);");
            Sql("DBCC CHECKIDENT ('dbo.ResetPasswordLinks', RESEED, 1);");
            Sql("DBCC CHECKIDENT ('dbo.JobTypes ', RESEED, 1);");
            Sql("DBCC CHECKIDENT ('dbo.Comments', RESEED, 1);");
        }

        public override void Down()
        {
            DropForeignKey("dbo.ResetPasswordLinks", "UserId", "dbo.Users");
            DropForeignKey("dbo.Comments", "PostTypeId", "dbo.PostTypes");
            DropForeignKey("dbo.JobPosts", "UserId", "dbo.Users");
            DropForeignKey("dbo.Salaries", "PostId", "dbo.JobPosts");
            DropForeignKey("dbo.Salaries", "SalaryTypeId", "dbo.SalaryTypes");
            DropForeignKey("dbo.JobPosts", "PlaceId", "dbo.Places");
            DropForeignKey("dbo.JobPosts", "JobTypeId", "dbo.JobTypes");
            DropForeignKey("dbo.JobDays", "PostId", "dbo.JobPosts");
            DropForeignKey("dbo.JobPosts", "ContractTypeId", "dbo.ContractTypes");
            DropForeignKey("dbo.Comments", "PostId", "dbo.JobPosts");
            DropForeignKey("dbo.Comments", "UserId", "dbo.Users");
            DropIndex("dbo.ResetPasswordLinks", new[] { "UserId" });
            DropIndex("dbo.Salaries", new[] { "SalaryTypeId" });
            DropIndex("dbo.Salaries", new[] { "PostId" });
            DropIndex("dbo.JobPosts", new[] { "PlaceId" });
            DropIndex("dbo.JobPosts", new[] { "UserId" });
            DropIndex("dbo.JobPosts", new[] { "ContractTypeId" });
            DropIndex("dbo.JobPosts", new[] { "JobTypeId" });
            DropIndex("dbo.JobDays", new[] { "PostId" });
            DropIndex("dbo.Comments", new[] { "PostTypeId" });
            DropIndex("dbo.Comments", new[] { "PostId" });
            DropIndex("dbo.Comments", new[] { "UserId" });
            DropTable("dbo.DayCategories");
            DropTable("dbo.WeeklySalaryCategories");
            DropTable("dbo.ResetPasswordLinks");
            DropTable("dbo.PostTypes");
            DropTable("dbo.SalaryTypes");
            DropTable("dbo.Salaries");
            DropTable("dbo.Places");
            DropTable("dbo.JobTypes");
            DropTable("dbo.JobPosts");
            DropTable("dbo.JobDays");
            DropTable("dbo.HourlySalaryCategories");
            DropTable("dbo.ContractTypes");
            DropTable("dbo.Users");
            DropTable("dbo.Comments");
        }
    }
}
