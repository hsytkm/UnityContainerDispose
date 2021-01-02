using System;
using System.Linq;
using System.Collections.Generic;
using Unity;
using Unity.Lifetime;
using Unity.Injection;
using Unity.Resolution;

namespace ConsoleApp1
{

    public sealed class MyUnityContainerWrapper : IUnityContainer, IDisposable
    {
        private readonly IUnityContainer _container;
        private readonly List<IDisposable> _disposables = new();

        public MyUnityContainerWrapper(IUnityContainer c) => _container = c.CreateChildContainer();

        public T Resolve<T>()
        {
            var obj = _container.Resolve<T>();
            if (obj is IDisposable d)
            {
                var type = typeof(T);
                var registration = _container.Registrations.FirstOrDefault(x => x.RegisteredType == type);

                // https://github.com/unitycontainer/abstractions/blob/master/src/Lifetime/Abstracts/LifetimeManager.cs#L28
                if (!registration?.LifetimeManager.InUse ?? false)
                    _disposables.Add(d);
            }
            return obj;
        }

        #region IUnityContainer
        public IEnumerable<IContainerRegistration> Registrations => _container.Registrations;

        public IUnityContainer Parent => _container.Parent;

        public IUnityContainer RegisterType(Type registeredType, Type mappedToType, string name, ITypeLifetimeManager lifetimeManager, params InjectionMember[] injectionMembers)
            => _container.RegisterType(registeredType, mappedToType, name, lifetimeManager, injectionMembers);

        public IUnityContainer RegisterInstance(Type type, string name, object instance, IInstanceLifetimeManager lifetimeManager)
            => _container.RegisterInstance(type, name, instance, lifetimeManager);

        public IUnityContainer RegisterFactory(Type type, string name, Func<IUnityContainer, Type, string, object> factory, IFactoryLifetimeManager lifetimeManager)
            => _container.RegisterFactory(type, name, factory, lifetimeManager);

        public bool IsRegistered(Type type, string name)
            => _container.IsRegistered(type, name);

        public object Resolve(Type type, string name, params ResolverOverride[] overrides)
            => _container.Resolve(type, name, overrides);

        public object BuildUp(Type type, object existing, string name, params ResolverOverride[] overrides)
            => _container.BuildUp(type, existing, name, overrides);

        public IUnityContainer CreateChildContainer()
            => _container.CreateChildContainer();
        #endregion

        private bool _disposed;
        public void Dispose()
        {
            if (!_disposed)
            {
                foreach (var d in _disposables) d.Dispose();
                _disposables.Clear();
                _container.Dispose();

                _disposed = true;
            }
        }
    }
}
