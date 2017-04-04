using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZSZ.Service.Entities;
using System.Data.Entity.ModelConfiguration;

namespace ZSZ.Service.ModelConfig
{
  public class RegionConfig:EntityTypeConfiguration<RegionEntity>
  {
    public RegionConfig()
    {
      this.ToTable("T_Regions");
      this.Property(p => p.Name).IsRequired().HasMaxLength(250);
      this.HasRequired(p => p.City).WithMany().HasForeignKey(p => p.CItyId)
        .WillCascadeOnDelete(false);
    }
  }
}
