using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZSZ.Common
{
  public class Pager
  {
    public int PageSize { get; set; }

    public int TotalCount { get; set; }

    public int MaxPagerCount { get; set; }
    //Begin from 1
    public int PageIndex { get; set; }
    //The format of url, and there is an appointment that {pn} was used as placeholder
    public string UrlPattern { get; set; }

    public string PlaceholderInUrl { get; set; } = "{pn}";
    //Css style
    public string CurrentPageClassName { get; set; }

    public string GetPagerHtml()
    {
      StringBuilder html = new StringBuilder();
      html.Append("<ul>");

      int pageCount = (int)Math.Ceiling(TotalCount * 1.0 / PageSize);
      //Compute the begin page been displayed
      int startPageIndex = Math.Max(1, PageIndex - MaxPagerCount / 2);
      //Compute the end page been displayed
      int endPageIndex = Math.Max(pageCount, startPageIndex + MaxPagerCount / 2);

      for (int i = startPageIndex; i <= endPageIndex; i++)
      {
        if (i == PageIndex)  //If this is current page, we set special css style
        {
          html.Append("<li class='>").Append(CurrentPageClassName).Append("'>")
            .Append(i).Append("</li>");
        }
        else
        {
          html.Append("<li><a href='").Append(UrlPattern.Replace(PlaceholderInUrl, i.ToString())).Append("'>")
            .Append(i).Append("</a></li>");
        }
      }
      html.Append("</ul>");
      return html.ToString();
    }
  }
}
