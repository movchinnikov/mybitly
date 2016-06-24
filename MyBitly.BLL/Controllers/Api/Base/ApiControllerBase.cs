namespace MyBitly.BLL.Controllers.Api.Base
{
    using System.Web.Http;
    using Castle.Windsor;

    public abstract class ApiControllerBase : ApiController
    {
        protected IWindsorContainer Container { get; private set; }

        protected ApiControllerBase(IWindsorContainer container)
        {
            this.Container = container;
        }
    }
}