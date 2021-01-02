using System;
using Unity;

namespace ConsoleApp1
{
    class Test2: Test1
    {
        protected override void DoSomething(IUnityContainer containerSource)
        {
            Console.WriteLine("Begin resove types");

            var containerWrapper = new MyUnityContainerWrapper(containerSource.CreateChildContainer());
            containerWrapper.RegisterType<IWriter, Writer>(/*TypeLifetime.Transient*/);

            Console.WriteLine("Begin resove types");
            var s1 = containerWrapper.Resolve<IService>();
            var s2 = containerWrapper.Resolve<IService>();
            Console.WriteLine("  IService is " + ((s1.Guid == s2.Guid) ? "Singleton." : "Transient."));

            var r1 = containerWrapper.Resolve<IReader>();
            var r2 = containerWrapper.Resolve<IReader>();
            Console.WriteLine("  IReader  is " + ((r1.Guid == r2.Guid) ? "Singleton." : "Transient."));
            Console.WriteLine("End resove types");

            var w1 = containerWrapper.Resolve<IWriter>();
            var w2 = containerWrapper.Resolve<IWriter>();
            Console.WriteLine("  IWriter  is " + ((w1.Guid == w2.Guid) ? "Singleton." : "Transient."));
            Console.WriteLine("End resove types");

            Console.WriteLine("Dispose MyUnityContainerWrapper");
            containerWrapper.Dispose();
        }
    }

}
