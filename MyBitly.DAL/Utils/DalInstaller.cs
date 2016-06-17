namespace MyBitly.DAL.Utils
{
    using System.Data.Entity;
    using Castle.MicroKernel.Registration;
    using Castle.MicroKernel.SubSystems.Configuration;
    using Castle.Windsor;
    using Repositories;
    using UnitOfWork;

    public class DalInstaller : IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            container.Register(
                Component.For<DbContext>().ImplementedBy<MyBitlyContext>().LifestylePerWebRequest(),
                Component.For<EfUnitOfWorkInterceptor>().LifestyleTransient(),
                Component.For<IUrlRepository>().ImplementedBy<UrlRepository>()
                    .Interceptors<EfUnitOfWorkInterceptor>().LifestyleTransient()
            );
        }
    }
}