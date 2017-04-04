using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity.ModelConfiguration;
using ZSZ.Service.Entities;

namespace ZSZ.Service.ModelConfig
{
  public class UserConfig : EntityTypeConfiguration<UserEntity>
  {
    public UserConfig()
    {
      this.ToTable("T_Users");
      this.HasRequired(u => u.City).WithMany().HasForeignKey(u => u.CityId).WillCascadeOnDelete(false);
      this.Property(p => p.PasswordHash).IsRequired().HasMaxLength(100);
      this.Property(p => p.PasswordSalt).IsRequired().HasMaxLength(20);
      this.Property(p => p.PhoneNum).IsRequired().HasMaxLength(20).IsUnicode(false);
    }
  }
}
