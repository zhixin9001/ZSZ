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
    }
  }
}
