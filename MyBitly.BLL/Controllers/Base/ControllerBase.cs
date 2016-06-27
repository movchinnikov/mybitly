namespace MyBitly.BLL.Controllers.Base
{
    using System.Web.Mvc;
    using Castle.Windsor;

    public abstract class ControllerBase : Controller
    {
        protected IWindsorContainer Container { get; private set; }

        protected ControllerBase(IWindsorContainer container)
        {
            Container = container;
        }
    }
}