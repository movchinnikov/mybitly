namespace MyBitly.BLL.Utils
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Http.Dependencies;
    using Castle.MicroKernel;
    using Castle.MicroKernel.Lifestyle;

    public class CustomDependencyScope : IDependencyScope
    {
        private readonly IKernel _container;

        private readonly IDisposable _scope;

        public CustomDependencyScope(IKernel container)
        {
            this._container = container;
            this._scope = container.BeginScope();
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
            this._scope.Dispose();
        }
    }
}