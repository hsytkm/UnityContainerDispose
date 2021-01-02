using System;
using Unity;

namespace UnityContainerDispose
{
    class Program
    {
        static void Main(string[] args)
        {
            Test();
        }

        static void Test()
        {
            using IUnityContainer container = new UnityContainer();

            container.RegisterType<IService, Service>(TypeLifetime.PerContainer);
            container.RegisterType<IReader, Reader>(/*TypeLifetime.Transient*/);

            // DIコンテナからインスタンスを取得して何かする
            DoSomething(container);
        }

        static void DoSomething(IUnityContainer container)
        {
            var s1 = container.Resolve<IService>();
            var s2 = container.Resolve<IService>();
            Console.WriteLine("IService is " + ((s1.Guid == s2.Guid) ? "Singleton." : "Transient."));

            var r1 = container.Resolve<IReader>();
            var r2 = container.Resolve<IReader>();
            Console.WriteLine("IReader  is " + ((r1.Guid == r2.Guid) ? "Singleton." : "Transient."));
        }
    }
}
