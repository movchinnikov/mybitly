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
			_container = container;
		}

		public IDependencyScope BeginScope()
		{
			return new CustomDependencyScope(_container);
		}

		public object GetService(Type serviceType)
		{
			return _container.HasComponent(serviceType) ? _container.Resolve(serviceType) : null;
		}

		public IEnumerable<object> GetServices(Type serviceType)
		{
			return _container.ResolveAll(serviceType).Cast<object>();
		}

		public void Dispose()
		{
		}
	}
}