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
 *    
 *      ⇒ ユーザもDIコンテナから管理型のライフサイクルを取得できるから、
 *         それに応じてええ感じに管理せい ってこと？？
 *
 * 4. 3を受け入れるとして『この型は Transient で管理されてるからDisposeしなちゃ』の状態から、
 *    設計変更で Register のライフサイクルを Transient から Singleton に変更した場合、
 *    他でも使用されているシングルトンのインスタンスを Dispose しちゃわない？
 *    逆もある（Singleton から Transient に変えたので、ユーザで Dispose して）
 * 
 *      ⇒ 開発途中にライフサイクルを変えるなってこと？？
 * 
 * 
 * 4. container.RegisterType<IReader, Reader>()
 *    の場合、取得インスタンスが IDispose を継承してるか見なあかんってこと？
 * 
 */
namespace ConsoleApp1
{
    class Program
    {
        static void Main(string[] args)
        {
            Test1.DoTest();
        }
    }
}
