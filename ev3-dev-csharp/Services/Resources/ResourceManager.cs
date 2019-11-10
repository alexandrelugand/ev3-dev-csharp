using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using log4net;

namespace EV3.Dev.Csharp.Services.Resources
{
	public class ResourceManager : IResourceManager
	{
	    private readonly ILog _logger;
	    private readonly DirectoryInfo _resourcesDirectoryInfo;

	    public ResourceManager(ILog logger)
	    {
		    _logger = logger;
		    _resourcesDirectoryInfo =
			    new DirectoryInfo(Path.Combine(Path.Combine(Assembly.GetExecutingAssembly().Location), "resources"));
	    }

	    public IDictionary<string, string> GetSounds()
	    {
		    _logger.Info("Explorer sound files ...");
			var soundFiles = new Dictionary<string, string>(StringComparer.InvariantCultureIgnoreCase);
		    foreach (var soundFileInfo in _resourcesDirectoryInfo.GetFiles("*.rsf"))
		    {
				_logger.Info($"Found '{soundFileInfo.FullName}' sound files");
			    if(!soundFiles.ContainsKey(soundFileInfo.Name))
					soundFiles.Add(soundFileInfo.Name, soundFileInfo.FullName);
		    }
		    return soundFiles;

	    }
    }
}
