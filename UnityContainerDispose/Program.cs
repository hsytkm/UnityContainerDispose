using System;

/* 1. DIコンテナのライフサイクルが PerContainer(Singleton) の場合、
 *    DIコンテナがインスタンスの参照を保持しており、
 *    DIコンテナの Dispose 時に保持インスタンスを Dipsose してくれる。
 *    
 * 2. DIコンテナのライフサイクルが Transient の場合は、
 *    Singleton のように生成インスタンスを Dipsose してくれないので、
 *    ResolveType() でインスタンスを取得したユーザが Dispose する必要がある。
 *    
 *      ⇒ ここまでは Qiita でまとめ済みの内容
 *
 * 3. ってことは、ResolveType するユーザは、取得インスタンスがDIコンテナで
 *    どのようなライフサイクルで管理されているかを認識する必要があるってこと？
 *    （ユーザもDIコンテナから管理さている型のライフサイクルを取得できるから、
 *    　それに応じてええ感じに管理せいってこと？？）
 * 
 * 4. 『IService は IDisposable ではないが、Service は IDisposeable』を考慮すると、
 *    Resolve<>() した場合は常に取得インスタンスの IDispose 継承をチェックして
 *    管理せなアカンってこと？  手間が掛かるなぁ…
 *      ----------------------------------------------------
 *        interface IService {}
 *        class Service : IService, IDisposable {}
 * 
 *        container.RegisterType<IService, Service>();
 * 
 *        var x = container.Resolve<IService>();
 *        
 *        if (x is IDisposable d) d.Dispose();
 *      ----------------------------------------------------
 * 
 */
namespace ConsoleApp1
{
    class Program
    {
        static void Main(string[] args)
        {
            new Test1().DoTest();
            Console.WriteLine(Environment.NewLine);

            new Test2().DoTest();
        }
    }
}
