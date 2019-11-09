namespace EV3.Dev.Csharp.Services
{
	public interface IEv3Services
	{
		T GetService<T>() where T : class;
	}
}