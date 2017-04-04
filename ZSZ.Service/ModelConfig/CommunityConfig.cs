using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZSZ.Service.Entities;
using System.Data.Entity.ModelConfiguration;

namespace ZSZ.Service.ModelConfig
{
 public class CommunityConfig:EntityTypeConfiguration<CommunityEntity>
      {
    public CommunityConfig()
    {
      this.ToTable("T_Communities");
      this.Property(p => p.Name).IsRequired().HasMaxLength(250);
      this.Property(p => p.Location).IsRequired().HasMaxLength(1024);
      this.Property(p => p.Traffic).IsRequired().HasMaxLength(1024);
      this.Property(p => p.BuiltYear).IsRequired();

      this.HasRequired(p => p.Region).WithMany().HasForeignKey(p => p.RegionId).WillCascadeOnDelete(false);
    }
  }
}
