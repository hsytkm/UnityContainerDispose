## UnityContainerDispose

UnityContainer に登録した型のインスタンスについて悩んでる議事ソースコード。



### 悩みの流れ

1. DIコンテナのライフサイクルが PerContainer(Singleton) の場合、DIコンテナがインスタンスの参照を保持しており、DIコンテナの Dispose 時に保持インスタンスを Dipsose してくれる。
   
   
   
2. DIコンテナのライフサイクルが Transient の場合は、Singleton のように生成インスタンスを Dipsose してくれないので、ResolveType\<T>() でインスタンスを取得したユーザが Dispose する必要がある。

     ⇒ ここまでは [Qiita](https://qiita.com/hsytkm/items/27d053d5daa2e75a48e7) でまとめ済みの内容

   

3. ってことは、ResolveType するユーザは、取得インスタンスがDIコンテナでどのようなライフサイクルで管理されているかを認識する必要があるってこと？

   ユーザもDIコンテナから管理さている型のライフサイクルを取得できるから、れに応じてええ感じに管理せいってこと？？

   

4. 『IService は IDisposable ではないが、Service は IDisposeable』を考慮すると、Resolve\<T>() した場合は常に取得インスタンスの IDispose 継承をチェックして管理せなアカンってこと？  手間が掛かるなぁ…

   ```C#
   interface IService {}
   class Service : IService, IDisposable {}
   
   container.RegisterType<IService, Service>();
   
   var x = container.Resolve<IService>();
   
   if (x is IDisposable d) d.Dispose();
   ```
   

## 関連ウェブページ

以下に同じ悩みが書いてあるが答えは出てない…

[Disposing needed in Unity? - stackoverflow](https://stackoverflow.com/questions/13581655/disposing-needed-in-unity)

TransientLifetimeManager の場合、 Resolve の度に新しいインスタンスが生成されます。その場合はユーザが Dispose を行う必要があります。

⇒ つまりユーザがコンテナに登録されている ITypeLifetimeManager を意識して Dispose() をコールせいとゆーてる？ 登録側が ライブタイムを変更した場合、他で使用されてるシングルトンオブジェクトを破棄してまうやん。おかしくない？



EOF