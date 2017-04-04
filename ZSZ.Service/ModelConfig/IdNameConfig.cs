using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity.ModelConfiguration;
using ZSZ.Service.Entities;

namespace ZSZ.Service.ModelConfig
{
  public class IdNameConfig : EntityTypeConfiguration<IdNameEntity>
  {
    public IdNameConfig()
    {
      this.ToTable("T_IdNames");
      this.Property(p => p.Name).IsRequired().HasMaxLength(1024);
      this.Property(p => p.TypeName).IsRequired().HasMaxLength(1024);
    }
  }
}
