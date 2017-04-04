using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZSZ.Service.Entities;

namespace ZSZ.Service.ModelConfig
{
  public class CityConfig : EntityTypeConfiguration<CityEntity>
  {
    public CityConfig()
    {
      this.ToTable("T_Cities");
      this.Property(p => p.Name).IsRequired().HasMaxLength(250);
    }
  }
}
