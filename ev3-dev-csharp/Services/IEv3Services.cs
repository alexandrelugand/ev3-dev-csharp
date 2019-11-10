using System;

namespace EV3.Dev.Csharp.Services
{
	public interface IEv3Services : IDisposable
	{
		T GetService<T>() where T : class;
	}
}