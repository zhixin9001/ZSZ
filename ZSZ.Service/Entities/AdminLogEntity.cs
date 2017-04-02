using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZSZ.Service.Entities
{
  public class AdminLogEntity : BaseEntity
  {
    public long UserId { get; set; }
    public string Msg { get; set; }
  }
}
