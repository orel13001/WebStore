using Microsoft.AspNetCore.Mvc;
using SimpleMvcSitemap;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebStore.Interfaces.Services;

namespace WebStore.Controllers.API
{
	public class SiteMapController : ControllerBase //localhost/SiteMap
	{
		public IActionResult Index([FromServices] IProductData ProductData)
		{
			var nodes = new List<SitemapNode>
			{
				new (Url.Action("Index", "Home")),
				new (Url.Action("ConfiguredAction", "Home")),
				new (Url.Action("Index", "Blogs")),
				new (Url.Action("Blog", "Blogs")),
				new (Url.Action("Index", "WebAPI")),
				new (Url.Action("Index", "Catalog")),
			};

			nodes.AddRange(ProductData.GetSections().Select(s => new SitemapNode(Url.Action("Index", "Catalog", new {SectionId = s.Id}))));
			nodes.AddRange(ProductData.GetBrands().Select(b => new SitemapNode(Url.Action("Index", "Catalog", new {BrandId = b.Id}))));
			nodes.AddRange(ProductData.GetProducts().Select(p => new SitemapNode(Url.Action("Index", "Catalog", new {p.Id}))));


			return new SitemapProvider().CreateSitemap(new SitemapModel(nodes));
		}
	}
}
