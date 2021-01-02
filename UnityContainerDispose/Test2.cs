using System;
using System.Linq;
using System.Collections.Generic;
using Unity;

namespace ConsoleApp1
{
    class Test2: Test1
    {
        protected override void DoSomething(IUnityContainer containerSource)
        {
            Console.WriteLine("Begin resove types");

            var container = new MyContainerWrapper(containerSource);

            var s1 = container.Resolve<IService>();
            var s2 = container.Resolve<IService>();
            Console.WriteLine("  IService is " + ((s1.Guid == s2.Guid) ? "Singleton." : "Transient."));

            var r1 = container.Resolve<IReader>();
            var r2 = container.Resolve<IReader>();
            Console.WriteLine("  IReader  is " + ((r1.Guid == r2.Guid) ? "Singleton." : "Transient."));
            Console.WriteLine("End resove types");

            Console.WriteLine("Dispose MyContainerWrapper");
            container.Dispose();
        }
    }

    class MyContainerWrapper : IDisposable
    {
        private readonly IUnityContainer _container;
        private readonly List<IDisposable> _disposables = new();

        public MyContainerWrapper(IUnityContainer c) => _container = c.CreateChildContainer();

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

        public void Dispose()
        {
            foreach (var d in _disposables)
            {
                d.Dispose();
            }
            _disposables.Clear();
            _container.Dispose();
        }
    }
}
