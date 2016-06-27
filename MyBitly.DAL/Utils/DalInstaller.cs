namespace MyBitly.DAL.Utils
{
	using System.Data.Entity;
	using Castle.MicroKernel.Registration;
	using Castle.MicroKernel.SubSystems.Configuration;
	using Castle.Windsor;
	using Factory;
	using Repositories;
	using UnitOfWork;

	public class DalInstaller : IWindsorInstaller
	{
		public void Install(IWindsorContainer container, IConfigurationStore store)
		{
			container.Register(
				Component.For<ISessionFactory>().ImplementedBy<SessionFactory>().LifestyleSingleton(),
				Component.For<DbContext>().ImplementedBy<MyBitlyContext>().LifestyleTransient(),
				Component.For<EfUnitOfWorkInterceptor>().LifestyleTransient(),
				Component.For<IUrlRepository>().ImplementedBy<UrlRepository>()
					.Interceptors<EfUnitOfWorkInterceptor>().LifestyleTransient()
			);
		}
	}
}