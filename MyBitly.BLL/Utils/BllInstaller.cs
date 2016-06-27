namespace MyBitly.BLL.Utils
{
	using System.Web.Http;
	using System.Web.Mvc;
	using Castle.MicroKernel.Registration;
	using Castle.MicroKernel.SubSystems.Configuration;
	using Castle.Windsor;
	using Common.Params;
	using Services;

	public class BllInstaller : IWindsorInstaller
	{
		public void Install(IWindsorContainer container, IConfigurationStore store)
		{
			container.Register(Classes.FromThisAssembly().BasedOn<IController>().LifestyleTransient());
			container.Register(Classes.FromThisAssembly().BasedOn<ApiController>().LifestyleTransient());

			container.Register(
				Component.For<IUrlService>().ImplementedBy<UrlService>().LifestyleSingleton(),
				Component.For<IParamsHelper>().ImplementedBy<ParamsHelper>().LifestyleSingleton()
				);
		}
	}
}