namespace MyBitly.BLL.Controllers
{
    using System.Web.Mvc;
    using Base;
    using Castle.Windsor;
    using Services;
    using Utils;

    public class HomeController : BaseController
    {
        private readonly IUrlService _urlService;

        public HomeController(IWindsorContainer container, IUrlService urlService)
            : base(container)
        {
            this._urlService = urlService;
        }

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult RedirectToLong(string hash)
        {
            try
            {
                var response = this._urlService.Get(hash);
                Helper.ShallowExceptions(() => this._urlService.Increment(hash));

                this._urlService.Increment(hash);
                return Redirect(response.LongUrl);
            }
            finally
            {
                if (this._urlService != null) this.Container.Release(this._urlService);
            }
        }
    }
}