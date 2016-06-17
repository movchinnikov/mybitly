namespace MyBitly.Front
{
    using System.Web;
    using System.Web.Http;
    using System.Web.Mvc;
    using System.Web.Routing;
    using BLL;
    using BLL.Utils;
    using Castle.MicroKernel.Registration;
    using Castle.Windsor;
    using DAL.Utils;

    public class WebApiApplication : HttpApplication
    {
        private IWindsorContainer _container;

        protected void Application_Start()
        {
            this._container = new WindsorContainer();
            this._container.Register(
                Component.For<IWindsorContainer>().Instance(this._container))
                .Install(new BllInstaller(), new DalInstaller());

            AreaRegistration.RegisterAllAreas();

            WebApiConfig.Register(GlobalConfiguration.Configuration);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);

            var controllerFactory = new CustomControllerFactory(this._container.Kernel);
            ControllerBuilder.Current.SetControllerFactory(controllerFactory);
            GlobalConfiguration.Configuration.DependencyResolver = new CustomDependencyResolver(this._container.Kernel);
        }

        protected void Application_End()
        {
            if (this._container != null) this._container.Dispose();
        }
    }
}