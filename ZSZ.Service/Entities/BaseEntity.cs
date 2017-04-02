﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZSZ.Service.Entities
{
  public class BaseEntity
  {
    public long Id { get; set; }
    public bool IsDeleted { get; set; } = false;
    public DateTime CreateDateTime { get; set; } = DateTime.Now;
  }
}
