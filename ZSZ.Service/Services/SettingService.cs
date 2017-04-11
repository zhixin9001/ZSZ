using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZSZ.DTO;
using ZSZ.IService;
using ZSZ.Service.Entities;
using System.Data.Entity;
namespace ZSZ.Service.Services
{
  public class SettingService : ISettingService
  {
    private ZszDBContext _ctx = new ZszDBContext();

    ~SettingService()
    {
      _ctx.Dispose();
    }

    public SettingDTO[] GetAll()
    {
      var cs = new CommonService<SettingEntity>(_ctx);
      return cs.GetAll().ToList().Select(s => ToDTO(s)).ToArray();
    }


    public bool? GetBoolValue(string name)
    {
      var strValue = GetValue(name);
      if (string.IsNullOrEmpty(strValue))
      {
        return null;
      }
      else
      {
        return Convert.ToBoolean(strValue);
      }
    }

    public int? GetIntValue(string name)
    {
      var strValue = GetValue(name);
      if (string.IsNullOrEmpty(strValue))
      {
        return null;
      }
      else
      {
        return Convert.ToInt32(strValue);
      }
    }

    public string GetValue(string name)
    {
      var cs = new CommonService<SettingEntity>(_ctx);
      var entity = cs.GetAll().AsNoTracking().FirstOrDefault(s => s.Name == name);
      if (entity == null)
      {
        return null;
      }
      else
      {
        return entity.Value;
      }
    }

    public void SetBoolValue(string name, bool value)
    {
      throw new NotImplementedException();
    }

    public void SetIntValue(string name, int value)
    {
      throw new NotImplementedException();
    }

    public void SetValue(string name, string value)
    {
      SettingEntity setting;
      var cs = new CommonService<SettingEntity>(_ctx);
      var entity = cs.GetAll().SingleOrDefault(s => s.Name == name);
      if (entity == null)//Add
      {
        setting = new SettingEntity()
        {
          Name = name,
          Value = value
        };
        _ctx.Settings.Add(setting);
      }
      else//Edit
      {
        setting = entity;
        setting.Value = value;
      }
      _ctx.SaveChanges();
    }

    private SettingDTO ToDTO(SettingEntity entity)
    {
      if (entity == null) return null;

      var dto = new SettingDTO()
      {
        Id = entity.Id,
        Name = entity.Name,
        CreateDateTime = entity.CreateDateTime,
        Value = entity.Value
      };

      return dto;
    }
  }
}
