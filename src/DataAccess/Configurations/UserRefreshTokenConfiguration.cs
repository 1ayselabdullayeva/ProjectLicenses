using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Configurations
{
    public class UserRefreshTokenConfiguration : IEntityTypeConfiguration<UserRefreshToken>
    {
        public void Configure(EntityTypeBuilder<UserRefreshToken> builder)
        {
            builder.Property(x=>x.RefreshToken).IsRequired();
            builder.HasOne(p => p.User).WithMany(p => p.RefreshTokens).HasForeignKey(p => p.UserId)
              .HasConstraintName("FK_RefreshTokens_User_Id");
        }
    }
}
