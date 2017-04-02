using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZSZ.Service.Entities
{
  public class HouseAppointEntity : BaseEntity
  {
    public long UserId { get; set; }
    public string Name { get; set; }
    public string PhoneNum { get; set; }
    public DateTime VisitDate { get; set; }
    public long HouseId { get; set; }
    public string Status { get; set; }
    public long FollowAdminUserId { get; set; }
    public DateTime FollowDateTime { get; set; }
  }
}
