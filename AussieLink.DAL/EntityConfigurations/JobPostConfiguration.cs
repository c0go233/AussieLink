using AussieLink.Contracts.Models.PostModels.JobPostModels;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AussieLink.DAL.EntityConfigurations
{
    internal class JobPostConfiguration : EntityTypeConfiguration<JobPost>
    {
        public JobPostConfiguration()
        {
            HasKey(p => p.PostId);

            Property(p => p.Title).IsRequired();

            Property(p => p.Description).IsRequired();

            HasMany(p => p.JobDays).WithRequired().HasForeignKey(d => d.PostId);

            HasMany(p => p.Salaries).WithRequired().HasForeignKey(s => s.PostId);

            HasMany(m => m.Comments)
                .WithRequired()
                .HasForeignKey(m => m.PostId)
                .WillCascadeOnDelete(false);
        }
    }
}
