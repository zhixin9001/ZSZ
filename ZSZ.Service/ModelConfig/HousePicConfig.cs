using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZSZ.Service.Entities;

namespace ZSZ.Service.ModelConfig
{
  public class HousePicConfig:EntityTypeConfiguration<HousePicEntity>
  {
    public HousePicConfig()
    {
      this.ToTable("T_HousePics");
      this.HasRequired(p => p.House).WithMany(p => p.HousePics).HasForeignKey(p => p.HouseId).WillCascadeOnDelete(false);
      this.Property(p => p.ThumbUrl).IsRequired().HasMaxLength(1024);
      this.Property(p => p.Url).IsRequired().HasMaxLength(1024);
    }
  }
}
