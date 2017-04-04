using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity.ModelConfiguration;
using ZSZ.Service.Entities.RBAC;

namespace ZSZ.Service.ModelConfig.RBAC
{
  public class RoleConfig:EntityTypeConfiguration<RoleEntity>
  {
    public RoleConfig()
    {
      this.ToTable("T_Roles");
      this.Property(p => p.Name).IsRequired().HasMaxLength(250);

      this.HasMany(p => p.Permissions).WithMany(p => p.Roles)
        .Map(m=>m.ToTable("T_RolePermissions")
        .MapLeftKey("RoleId")
        .MapRightKey("PermissionId"));
    }
  }
}
