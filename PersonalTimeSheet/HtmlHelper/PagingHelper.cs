using System;
using System.Text;
using System.Web.Mvc;
using PersonalTimeSheet.Models;

namespace PersonalTimeSheet.HtmlHelper
{
    public static class PagingHelper
    {
        public static MvcHtmlString PageLinks(this System.Web.Mvc.HtmlHelper html,
                                              PagingInfo info,
                                              Func<int, string> pageUrl)
        {
            var strBuiler = new StringBuilder();
            for (var i = 1; i <= info.TotalPages; i++)
            {
                var tagBuilder = new TagBuilder("a");
                tagBuilder.MergeAttribute("href", pageUrl(i));
                tagBuilder.InnerHtml = i.ToString();

                if (i == info.CurrentPage)
                {
                    tagBuilder.AddCssClass("selected");
                }

                strBuiler.Append(tagBuilder.ToString());
            }

            return MvcHtmlString.Create(strBuiler.ToString());
        }

    }
}