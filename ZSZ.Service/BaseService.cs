using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZSZ.Service.Entities;

namespace ZSZ.Service
{
  public class BaseService<T> where T : BaseEntity
  {
    public IQueryable<T> Get
  }
}
