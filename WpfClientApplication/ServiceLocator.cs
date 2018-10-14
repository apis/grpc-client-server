using System;
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace WpfClientApplication
{
	public class ServiceLocator : IServiceLocator
	{
		private static readonly Lazy<IServiceLocator> LazyInstance;

		private readonly IDictionary<Type, object> _servicesDictionary = new ConcurrentDictionary<Type, object>();

		static ServiceLocator()
		{
			LazyInstance = new Lazy<IServiceLocator>(() => new ServiceLocator());
		}

		public static IServiceLocator Instance => LazyInstance.Value;

		public T GetService<T>()
		{
			if (!_servicesDictionary.TryGetValue(typeof(T), out var instance))
				throw new Exception("Service is not registered!");

			return (T) instance;
		}

		public void RegisterService<T>(object instance)
		{
			if (_servicesDictionary.TryGetValue(typeof(T), out _))
				throw new Exception("Service already registered!");

			if (!(instance is T))
				throw new Exception("Instance is not derived from supplied Type!");

			_servicesDictionary.Add(typeof(T), instance);
		}
	}
}