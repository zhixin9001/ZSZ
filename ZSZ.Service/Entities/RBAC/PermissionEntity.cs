using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZSZ.Service.Entities.RBAC
{
  public class PermissionEntity:BaseEntity
  {
    public string Description { get; set; }
    public string Name { get; set; }

    public ICollection<RoleEntity> Roles { get; set; } = new List<RoleEntity>();
  }
}
