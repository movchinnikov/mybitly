namespace MyBitly.BLL
{
	using System.Web.Mvc;
	using System.Web.Routing;

	public class RouteConfig
	{
		public static void RegisterRoutes(RouteCollection routes)
		{
			routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

			routes.MapRoute(
				name: "Default",
				url: "",
				defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
			);

			routes.MapRoute(
				name: "URL",
				url: "{hash}",
				defaults: new { controller = "Home", action = "RedirectToLong", hash = UrlParameter.Optional }
			);

			routes.MapRoute(
				name: "Basic",
				url: "{controller}/{action}/{id}",
				defaults: new { controller = "Home", id = UrlParameter.Optional }
);
		}
	}
}