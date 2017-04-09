using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZSZ.DTO;

namespace ZSZ.IService
{
  public interface IHouseService : IServiceSupport
  {
    HouseDTO GetById(long id);

    long GetTotalCount(long cityId, long typeId);

    HouseDTO[] GetPagedData(long cityId, long typeId, int pageSize, int currentIndex);

    long AddNew(HouseDTO house);

    void Update(HouseDTO house);

    void MarkDeleted(long id);

    HousePicDTO[] GetPics(long houseId);

    long AddNewHousePic(HousePicDTO housePic);

    void DeleteHousePic(long housePicId);

    HouseSearchResult Search(HouseSearchOptions options);

    int GetCount(long cityId, DateTime startDateTime, DateTime endDateTime);
  }

  public class HouseSearchOptions
  {
    public long CityId { get; set; }//城市id
    public long TypeId { get; set; }//房源类型，可空
    public long? RegionId { get; set; }//区域，可空
    public int? StartMonthRent { get; set; }//起始月租，可空
    public int? EndMonthRent { get; set; }//结束月租，可空
    public OrderByType OrderByType { get; set; } = OrderByType.MonthRentAsc;//排序方式
    public String Keywords { get; set; }//搜索关键字，可空
    public int PageSize { get; set; }//每页数据条数
    public int CurrentIndex { get; set; }//当前页码
  }

  public enum OrderByType
  {
    MonthRentDesc = 1,
    MonthRentAsc = 2,
    AreaDesc = 4,
    AreaAsc = 8,
    CreateDateDesc = 16
  }

  public class HouseSearchResult
  {
    public HouseDTO[] result { get; set; }//当前页的数据
    public long totalCount { get; set; }//搜索的结果总条数
  }
}
