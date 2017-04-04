using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity.ModelConfiguration;
using ZSZ.Service.Entities;

namespace ZSZ.Service.ModelConfig
{
 public class HouseAppointmentConfig:EntityTypeConfiguration<HouseAppointEntity>
  {
    public HouseAppointmentConfig()
    {
      this.ToTable("T_HouseAppointments");
      this.HasRequired(h => h.User).WithMany().HasForeignKey(h => h.UserId).WillCascadeOnDelete(false);
      this.HasRequired(h => h.House).WithMany().HasForeignKey(h => h.HouseId).WillCascadeOnDelete(false);
      this.HasOptional(h => h.FollowAdminUser).WithMany().HasForeignKey(h => h.FollowAdminUserId).WillCascadeOnDelete(false);
      this.Property(h => h.Name).IsRequired().HasMaxLength(20);
      this.Property(h => h.PhoneNum).IsRequired().HasMaxLength(20).IsUnicode(false);
      this.Property(h => h.Status).IsRequired().HasMaxLength(20);
    }
  }
}
