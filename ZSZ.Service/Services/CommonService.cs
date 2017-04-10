using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZSZ.Service.Entities;

namespace ZSZ.Service.Services
{
  public class CommonService<T> where T : BaseEntity
  {
    private ZszDBContext ctx;
    public CommonService(ZszDBContext ctx)
    {
      this.ctx = ctx;
    }
    public IQueryable<T> GetAll()
    {
      return ctx.Set<T>().Where(e => e.IsDeleted == false);
    }

    public long GetTotalCount()
    {
      return GetAll().LongCount();
    }

    public T GetById(long id)
    {
      return GetAll().Where(e => e.Id == id).SingleOrDefault();
    }

    public void MarkDeleted(long id)
    {
      var result = GetById(id);
      result.IsDeleted = true;
      ctx.SaveChanges();
    }

    public IQueryable<T> GetPagedDate(int startIndex, int num)
    {
      return GetAll().OrderBy(e => e.CreateDateTime).Skip(startIndex).Take(num);
    }
  }
}
