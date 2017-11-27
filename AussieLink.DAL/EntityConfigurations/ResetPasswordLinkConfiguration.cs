using AussieLink.Contracts.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AussieLink.DAL.EntityConfigurations
{
    internal class ResetPasswordLinkConfiguration : EntityTypeConfiguration<ResetPasswordLink>
    {
        public ResetPasswordLinkConfiguration()
        {
            HasKey(r => r.ResetPasswordLinkId);
        }
    }
}
