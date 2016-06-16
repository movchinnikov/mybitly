namespace MyBitly.BLL.Utils
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Http.Dependencies;
    using Castle.MicroKernel;
    using IDependencyResolver = System.Web.Http.Dependencies.IDependencyResolver;

    public class CustomDependencyResolver : IDependencyResolver
    {
        private readonly IKernel _container;

        public CustomDependencyResolver(IKernel container)
        {
            this._container = container;
        }

        public IDependencyScope BeginScope()
        {
            return new CustomDependencyScope(this._container);
        }

        public object GetService(Type serviceType)
        {
            return this._container.HasComponent(serviceType) ? this._container.Resolve(serviceType) : null;
        }

        public IEnumerable<object> GetServices(Type serviceType)
        {
            return this._container.ResolveAll(serviceType).Cast<object>();
        }

        public void Dispose()
        {
        }
    }
}