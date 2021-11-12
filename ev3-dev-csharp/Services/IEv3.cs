using System;
using System.Collections.Generic;
using Unity;

namespace EV3.Dev.Csharp.Services
{
    public interface IEv3 : IDisposable
    {
        void Init(Action<IUnityContainer> callback);
        IUnityContainer Container { get; }
        T Resolve<T>(string name = null) where T : class;
        object Resolve(Type type, string name = null);
        IEnumerable<T> ResolveAll<T>() where T : class;
        IEnumerable<object> ResolveAll(Type type);
    }
}