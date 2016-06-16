namespace MyBitly.BLL.Controllers.Api.Base
{
    using System.Web.Http;
    using Castle.Windsor;

    public class BaseApiController : ApiController
    {
        protected IWindsorContainer Container { get; private set; }

        public BaseApiController(IWindsorContainer container)
        {
            this.Container = container;
        }
    }
}