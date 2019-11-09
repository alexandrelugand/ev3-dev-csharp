using System;

namespace EV3.Dev.Csharp.Core.DesignPatterns
{
	public class Singleton<T> where T : class
	{
		private Singleton() { }

		private static readonly Lazy<T> LazyInstance = new Lazy<T>(() => Activator.CreateInstance(typeof(T), true) as T);

		public static T Instance => LazyInstance.Value;
	}
}
