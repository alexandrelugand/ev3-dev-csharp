using System.Collections.Generic;

namespace EV3.Dev.Csharp.Services.Resources
{
	public interface IResourceManager
	{
		IDictionary<string, string> GetSounds();
	}
}