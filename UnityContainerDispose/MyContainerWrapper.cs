using System;
using System.Linq;
using System.Collections.Generic;
using Unity;
using Unity.Lifetime;

namespace ConsoleApp1
{

    public sealed class MyUnityContainerWrapper : IDisposable
    {
        private readonly IUnityContainer _container;
        private readonly List<IDisposable> _disposables = new();

        public MyUnityContainerWrapper(IUnityContainer c) => _container = c;

        public IUnityContainer RegisterType<TFrom, TTo>() where TTo : TFrom
            => _container.RegisterType<TFrom, TTo>(TypeLifetime.Transient);

        public IUnityContainer RegisterType<TFrom, TTo>(ITypeLifetimeManager lifetimeManager) where TTo : TFrom
            => _container.RegisterType<TFrom, TTo>(lifetimeManager);

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
