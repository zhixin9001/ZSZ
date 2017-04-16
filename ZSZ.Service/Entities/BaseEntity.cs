using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZSZ.Service.Entities
{
  public abstract class BaseEntity
  {
    public long Id { get; set; }
    public bool IsDeleted { get; set; } = false;
    public DateTime CreateDateTime { get; set; } = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day
            , DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second);
  }
}
