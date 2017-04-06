using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZSZ.IService
{
  public interface ICityService : IServiceSupport
  {
    /// <summary>
    /// Add new city
    /// </summary>
    /// <param name="cityName"></param>
    /// <returns>the id of the added city</returns>
    long AddNew(string cityName);
  }
}
