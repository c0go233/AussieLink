using AussieLink.Contracts.Models;
using AussieLink.Contracts.Models.AdModels.JobAdModels;
using AussieLink.Contracts.Models.CommentModels;
using AussieLink.Contracts.Models.PostModels;
using AussieLink.Contracts.Models.PostModels.JobPostModels;
using AussieLink.Contracts.Models.PostModels.SharePostModels;
using AussieLink.DAL.EntityConfigurations;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AussieLink.DAL.Data
{
    public class DataContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<ResetPasswordLink> ResetPasswordLinks { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<PostType> PostTypes { get; set; }

        public DbSet<JobPost> JobPosts { get; set; }
        public DbSet<Place> Places { get; set; }
        public DbSet<ContractType> ContractTypes { get; set; }
        public DbSet<SalaryType> SalaryTypes { get; set; }
        public DbSet<Salary> Salaries { get; set; }
        public DbSet<JobDay> JobDays { get; set; }
        public DbSet<JobType> JobTypes { get; set; }
        public DbSet<WeeklySalaryCategory> WeeklySalaryCategories { get; set; }
        public DbSet<HourlySalaryCategory> HourlySalaryCategories { get; set; }

        public DbSet<Gender> Genders { get; set; }
        public DbSet<Suburb> Suburbs { get; set; }
        public DbSet<ShareType> ShareTypes { get; set; }
        public DbSet<Address> Addresses { get; set; }
        public DbSet<SharePost> SharePosts { get; set; }

        public DbSet<Picture> Pictures { get; set; }


        public DataContext() : base("DefaultConnection")
        {
            var ensureDLLIsCopied = System.Data.Entity.SqlServer.SqlProviderServices.Instance;
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Configurations.Add(new UserConfiguration());
            modelBuilder.Configurations.Add(new ResetPasswordLinkConfiguration());
            modelBuilder.Configurations.Add(new JobPostConfiguration());

            //configruation 
            modelBuilder.Entity<Place>().Property(p => p.Name).IsRequired();
            modelBuilder.Entity<Place>().HasMany(m => m.Suburbs).WithRequired().HasForeignKey(m => m.PlaceId);

            modelBuilder.Entity<ContractType>().Property(c => c.Name).IsRequired();
            modelBuilder.Entity<JobType>().Property(j => j.Name).IsRequired();
            modelBuilder.Entity<SalaryType>().Property(s => s.Name).IsRequired();
            modelBuilder.Entity<Salary>().Property(s => s.Size).IsRequired();
            modelBuilder.Entity<JobDay>().Property(d => d.Size).IsRequired();

            modelBuilder.Entity<DayCategory>().HasKey(d => d.DayCategoryId);
            modelBuilder.Entity<DayCategory>().Property(p => p.Name).IsRequired();

            modelBuilder.Entity<WeeklySalaryCategory>().Property(p => p.Name).IsRequired();
            modelBuilder.Entity<WeeklySalaryCategory>().HasKey(m => m.WeeklySalaryCategoryId);

            modelBuilder.Entity<HourlySalaryCategory>().Property(p => p.Name).IsRequired();
            modelBuilder.Entity<HourlySalaryCategory>().HasKey(p => p.HourlySalaryCategoryId);

            modelBuilder.Entity<Comment>().HasKey(m => m.CommentId);
            modelBuilder.Entity<Comment>().Property(m => m.Description).IsRequired();

            modelBuilder.Entity<PostType>().HasKey(m => m.PostTypeId);
            modelBuilder.Entity<PostType>().Property(m => m.Name).IsRequired();
            modelBuilder.Entity<PostType>().HasMany(m => m.Comments).WithRequired().HasForeignKey(m => m.PostTypeId).WillCascadeOnDelete(false);

            modelBuilder.Entity<Gender>().HasKey(m => m.GenderId);
            modelBuilder.Entity<Gender>().Property(m => m.Name).IsRequired();

            modelBuilder.Entity<Suburb>().HasKey(m => m.SuburbId);
            modelBuilder.Entity<Suburb>().Property(m => m.Name).IsRequired();

            modelBuilder.Entity<ShareType>().HasKey(m => m.ShareTypeId);
            modelBuilder.Entity<ShareType>().Property(m => m.Name).IsRequired();

            modelBuilder.Entity<Address>().HasKey(m => m.PostId);

            modelBuilder.Entity<SharePost>().HasKey(m => m.PostId);
            modelBuilder.Entity<SharePost>().Property(m => m.Title).IsRequired();
            modelBuilder.Entity<SharePost>().Property(m => m.Description).IsRequired();
            modelBuilder.Entity<SharePost>().HasRequired(m => m.Address).WithRequiredPrincipal(t => t.SharePost);
            modelBuilder.Entity<SharePost>().HasMany(m => m.Comments).WithRequired().HasForeignKey(m => m.PostId).WillCascadeOnDelete(false);
            modelBuilder.Entity<SharePost>().HasMany(m => m.Pictures).WithOptional().HasForeignKey(m => m.PostId);

            modelBuilder.Entity<Picture>().Property(m => m.PictureType).IsRequired();
        }
    }
}
