using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity.ModelConfiguration;
using ZSZ.Service.Entities;

namespace ZSZ.Service.ModelConfig
{
  public class AttachmentConfig:EntityTypeConfiguration<AttachmentEntity>
  {
    public AttachmentConfig()
    {
      this.ToTable("T_Attachments");
      this.HasMany(a => a.Houses).WithMany(a => a.Attachments)
        .Map(m=>m.ToTable("T_HouseAttachments")
        .MapLeftKey("AttachmentId")
        .MapRightKey("HouseId"));
      this.Property(e => e.IconName).IsRequired().HasMaxLength(50).IsUnicode(false);
      this.Property(e => e.Name).IsRequired().HasMaxLength(50);
    }
  }
}
