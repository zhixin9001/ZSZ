using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity.ModelConfiguration;
using ZSZ.Service.Entities.RBAC;

namespace ZSZ.Service.ModelConfig.RBAC
{
  public class PermissionConfig : EntityTypeConfiguration<PermissionEntity>
  {
    public PermissionConfig()
    {
      this.ToTable("T_Permissions");
      this.Property(p => p.Description).IsOptional().HasMaxLength(512);
      this.Property(p => p.Name).IsRequired().HasMaxLength(250);
    }
  }
}
