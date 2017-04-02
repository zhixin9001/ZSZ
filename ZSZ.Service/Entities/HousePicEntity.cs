using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZSZ.Service.Entities
{
  public class HousePicEntity : BaseEntity
  {
    public int HouseId { get; set; }
    public string Uri { get; set; }
    public string ThumbUri { get; set; }
  }
}
