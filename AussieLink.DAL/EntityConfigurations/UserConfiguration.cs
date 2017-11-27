using AussieLink.Contracts.Models;
using AussieLink.Contracts.Models.CommentModels;
using System.Data.Entity.ModelConfiguration;


namespace AussieLink.DAL.EntityConfigurations
{
    internal class UserConfiguration : EntityTypeConfiguration<User>
    {
        public UserConfiguration()
        {

            HasKey(m => m.UserId);

            Property(m => m.Email)
                .IsRequired()
                .HasMaxLength(100);

            Property(m => m.Name)
                .IsRequired()
                .HasMaxLength(50);


        }
    }
}
