namespace MyBitly.BLL.Controllers
{
    using System.Web.Mvc;
    using Castle.Windsor;
    using Base;
    using Services;
    using Utils;
    using ControllerBase = Base.ControllerBase;

    public class HomeController : ControllerBase
    {
        private readonly IUrlService _urlService;

        public HomeController(IWindsorContainer container, IUrlService urlService)
            : base(container)
        {
            _urlService = urlService;
        }

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult RedirectToLong(string hash)
        {
            try
            {
                var response = _urlService.Get(hash);
                Helper.ShallowExceptions(() => _urlService.Increment(hash));

                return Redirect(response.LongUrl);
            }
            finally
            {
                if (_urlService != null) Container.Release(_urlService);
            }
        }
    }
}