namespace MyBitly.BLL
{
    using System.Web.Http;

    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            config.Routes.MapHttpRoute("ActionApi",
                "api/{action}",
                new { controller = "url" },
                new {action = @"[a-zA-Z]+"});
        }
    }
}
