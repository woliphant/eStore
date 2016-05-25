using Microsoft.AspNet.Http;
using Microsoft.AspNet.Http.Features;
using Microsoft.AspNet.Razor.TagHelpers;
using System;
using System.Text;
using eStore.ViewModels;

namespace eStore.TagHelpers
{
    // You may need to install the Microsoft.AspNet.Razor.Runtime package into your project
    [HtmlTargetElement("catalogue", Attributes = BrandIdAttribute)]
    public class CatalogueHelper : TagHelper
    {
        private const string BrandIdAttribute = "brand";
        [HtmlAttributeName(BrandIdAttribute)]
        public string BrandId { get; set; }
        private readonly IHttpContextAccessor _httpContextAccessor;
        private ISession _session => _httpContextAccessor.HttpContext.Session;
        public CatalogueHelper(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }
        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            if (_session.GetObject<ProductViewModel[]>("order") != null && Convert.ToInt32(BrandId) > 0)
            {
                var innerHtml = new StringBuilder();
                ProductViewModel[] order = _session.GetObject<ProductViewModel[]>("order");
                innerHtml.Append("<div class=\"col-xs-12\" style=\"font-size:x-large;\"><span>Catalogue</span></div>");
                foreach (ProductViewModel item in order)
                {
                    if (item.BrandId == Convert.ToInt32(BrandId))
                    {
                        innerHtml.Append("<div id=\"item\" class=\"col-sm-3 col-xs-12 text-center\" style=\"border:solid;\">");
                        innerHtml.Append("<span class=\"col-xs-12\"><img class=\"img-responsive\" src=\"/img/" + item.GraphicName + ".png\" /></span>");
                        innerHtml.Append("<p id=descr" + item.Id + " data-description=\"" + item.Description + "\">");
                        innerHtml.Append("<span style=\"font-size:large;\">" + item.Description.Substring(0, 10) + "...</span></p><div>");
                        innerHtml.Append("<span>For Game Details.<br />Click Details</span></div>");
                        innerHtml.Append("<div style=\"padding-bottom: 10px;\"><a href=\"#details_popup\" data-toggle=\"modal\" class=\"btn btn-default\"");
                        innerHtml.Append(" id=\"modalbtn\" data-id=\"" + item.Id + "\">Details</a></div>");
                        innerHtml.Append("<input type=\"hidden\" id=\"pbrand" + item.Id + "\" value=\"" + item.BrandName + "\"/>");
                        innerHtml.Append("<input type=\"hidden\" id=\"pname" + item.Id + "\" value=\"" + item.ProductName + "\"/>");
                        innerHtml.Append("<input type=\"hidden\" id=\"pprice" + item.Id + "\" value=\"" + item.CostPrice + "\"/>");
                        innerHtml.Append("<input type=\"hidden\" id=\"pgraphic" + item.Id + "\" value=\"" + item.GraphicName + "\"/>");
                        innerHtml.Append("<input type=\"hidden\" id=\"pdescr" + item.Id + "\" value=\"" + item.Description + "\"/></div>");
                    }
                }
                output.Content.SetHtmlContent(innerHtml.ToString());
            }
        }
    }
}