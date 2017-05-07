using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ZSZ.FrontWeb.Models;
using ZSZ.IService;

namespace ZSZ.FrontWeb.Controllers
{
  public class HouseController : Controller
  {
    public IHouseService _HouseService { get; set; }
    public IAttachmentService _AttachmentService { get; set; }
    // GET: House
    public ActionResult Index(long id)
    {
      var house = _HouseService.GetById(id);
      if (house == null) return View("Error", (object)"The House isn't Exist");

      var pics = _HouseService.GetPics(id);
      var attachments = _AttachmentService.GetAttachments(id);

      var model = new HouseIndexViewModel
      {
        House = house,
        Pics = pics,
        Attachments = attachments
      };

      return View(model);
    }
  }
}