using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity.ModelConfiguration;
using ZSZ.Service.Entities;

namespace ZSZ.Service.ModelConfig
{
  public class AdminLogConfig:EntityTypeConfiguration<AdminLogEntity>
  {
    public AdminLogConfig()
    {
      this.ToTable("T_AdminLogs");
      this.HasRequired(l => l.AdminUser).WithMany()
          .HasForeignKey(e => e.AdminUserId).WillCascadeOnDelete(false);
      this.Property(e => e.Msg).IsRequired();
    }
  }
}
