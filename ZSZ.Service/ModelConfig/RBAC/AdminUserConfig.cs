using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity.ModelConfiguration;
using ZSZ.Service.Entities.RBAC;

namespace ZSZ.Service.ModelConfig.RBAC
{
 public class AdminUserConfig:EntityTypeConfiguration<AdminUserEntity>
  {
    public AdminUserConfig()
    {
      this.ToTable("T_AdminUsers");
      this.HasOptional(p => p.City).WithMany().HasForeignKey(p => p.CityId).WillCascadeOnDelete(false);
      this.HasMany(p => p.Roles).WithMany(r => r.AdminUsers)
        .Map(m=>m.ToTable("T_AdminUserRoles")
        .MapLeftKey("AdminUserId")
        .MapRightKey("RoleId"));

      this.Property(e => e.Name).HasMaxLength(50).IsRequired();
      this.Property(e => e.Email).HasMaxLength(30).IsRequired().IsUnicode(false);//varchar(30)
      this.Property(e => e.PhoneNum).HasMaxLength(20).IsRequired().IsUnicode(false);
      this.Property(e => e.PasswordSalt).HasMaxLength(20).IsRequired().IsUnicode(false);
      this.Property(e => e.PasswordHash).HasMaxLength(100).IsRequired().IsUnicode(false);
    }
  }
}
