using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ZSZ.AdminWeb.Models
{
  public class HouseAddModel
  {
    [Required]
    public long CommunityId { get; set; }
    [Required]
    public long RoomTypeId { get; set; }
    [Required]
    public string Address { get; set; }
    [Required]
    public int MonthRent { get; set; }
    [Required]
    public long StatusId { get; set; }
    [Required]
    public decimal Area { get; set; }
    [Required]
    public long DecorateStatusId { get; set; }
    [Required]
    public int FloorIndex { get; set; }
    [Required]
    public int TotalFloor { get; set; }
    [Required]
    public string Direction { get; set; }
    [Required]
    public DateTime LookableDateTime { get; set; }
    [Required]
    public DateTime CheckInDateTime { get; set; }
    [Required]
    public string OwnerName { get; set; }
    [Required]
    public string OwnerPhoneNum { get; set; }
    [Required]
    public string Description { get; set; }
    [Required]
    public long[] AttachmentIds { get; set; }
    [Required]
    public long TypeId { get; set; }
  }
}