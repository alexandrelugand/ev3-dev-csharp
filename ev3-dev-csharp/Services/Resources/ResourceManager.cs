using EV3.Dev.Csharp.Core.Helpers;
using log4net;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;

namespace EV3.Dev.Csharp.Services.Resources
{
	public class ResourceManager : IResourceManager
	{
		private readonly ILog _logger;
		private readonly Dictionary<string, string> _soundFiles = new Dictionary<string, string>(StringComparer.InvariantCultureIgnoreCase);

		public ResourceManager(ILog logger)
		{
			_logger = logger;
			// ReSharper disable once AssignNullToNotNullAttribute
			var resourcesDirectoryInfo = new DirectoryInfo(Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "resources"));
			if (resourcesDirectoryInfo.Exists)
			{
				LoadSounds(resourcesDirectoryInfo);
			}
		}

		private void LoadSounds(DirectoryInfo directoryInfo)
		{
			_logger.Debug($"Explore sound files in '{directoryInfo.FullName}' ...");
			foreach (var soundFileInfo in directoryInfo.GetFiles())
			{
				if(Path.GetExtension(soundFileInfo.Name).EqualsNoCase(".rsf"))
				{
					_logger.Debug($"Found '{soundFileInfo.FullName}' sound file");
					if (!_soundFiles.ContainsKey(Path.GetFileNameWithoutExtension(soundFileInfo.Name)))
						_soundFiles.Add(Path.GetFileNameWithoutExtension(soundFileInfo.Name), soundFileInfo.FullName);
				}
			}

			foreach (var subDirectoryInfo in directoryInfo.GetDirectories())
			{
				if (subDirectoryInfo.Name == "." || subDirectoryInfo.Name == "..")
					continue; 
				LoadSounds(subDirectoryInfo);
			}
		}

		public string GetSounds(string soundName)
		{
			if (_soundFiles.TryGetValue(soundName, out string s))
				return s;
			return string.Empty;
		}
	}
}
