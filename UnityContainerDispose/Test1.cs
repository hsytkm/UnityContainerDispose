﻿using System;
using Unity;

namespace ConsoleApp1
{
    class Test1
    {
        public void DoTest()
        {
            Console.WriteLine($"--- Start {this.GetType()} ---");
            var container = GetContainer();

            // DIコンテナからインスタンスを取得して何かする
            DoSomething(container);

            Console.WriteLine("Dispose IUnityContainer");
            container.Dispose();
        }

        static IUnityContainer GetContainer()
        {
            IUnityContainer container = new UnityContainer();

            Console.WriteLine("Begin register types");
            container.RegisterType<IService, Service>(TypeLifetime.PerContainer);
            container.RegisterType<IReader, Reader>(/*TypeLifetime.Transient*/);
            Console.WriteLine("End register types");

            foreach (var item in container.Registrations)
            {
                Console.WriteLine($"{item.RegisteredType} : {item.LifetimeManager.GetType()}");
            }

            return container;
        }

        protected virtual void DoSomething(IUnityContainer container)
        {
            Console.WriteLine("Begin resove types");

            var s1 = container.Resolve<IService>();
            var s2 = container.Resolve<IService>();
            Console.WriteLine("  IService is " + ((s1.Guid == s2.Guid) ? "Singleton." : "Transient."));

            var r1 = container.Resolve<IReader>();
            var r2 = container.Resolve<IReader>();
            Console.WriteLine("  IReader  is " + ((r1.Guid == r2.Guid) ? "Singleton." : "Transient."));
            Console.WriteLine("End resove types");
        }

    }
}
