namespace MyBitly.BLL.Controllers.Base
{
    using System.Web.Mvc;
    using Castle.Windsor;

    public class BaseController : Controller
    {
        protected IWindsorContainer Container { get; private set; }

        public BaseController(IWindsorContainer container)
        {
            this.Container = container;
        }
    }
}