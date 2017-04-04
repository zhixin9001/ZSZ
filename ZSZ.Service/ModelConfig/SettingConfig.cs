using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZSZ.Service.Entities;

namespace ZSZ.Service.ModelConfig
{
  public class SettingConfig:EntityTypeConfiguration<SettingEntity>
  {
    public SettingConfig()
    {
      this.ToTable("T_Settings");
      this.Property(p => p.Name).IsRequired().HasMaxLength(250);
      this.Property(p => p.Value).IsRequired();
    }
  }
}
