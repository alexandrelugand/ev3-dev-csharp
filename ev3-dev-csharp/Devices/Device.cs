using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using log4net;
using EV3.Dev.Csharp.Services;
using EV3.Dev.Csharp.Core.Helpers;

namespace EV3.Dev.Csharp.Devices
{
	public class Device
	{
		protected readonly Ev3Services Ev3Services;
		protected readonly ILog Logger;

		protected const string SysRoot = "/sys/";
		private int _deviceIndex = -1;


		public Device()
		{
			Ev3Services = Ev3Services.Instance;
			Logger = Ev3Services.GetService<ILog>();
		}

		protected string Path { get; set; }

		protected bool Connect(string deviceType, string classDir, string pattern, IDictionary<string, string[]> match)
		{
			if (!Directory.Exists(classDir))
				return false;

			Debug.WriteLine($"##### > Trying to connect device '{classDir}' with '{pattern}' pattern ...");
			var dirs = Directory.EnumerateDirectories(classDir);
			foreach (var currentFullDirPath in dirs)
			{
				var dirName = global::System.IO.Path.GetFileName(currentFullDirPath);
				if (dirName != null && dirName.StartsWith(pattern))
				{
					Path = global::System.IO.Path.Combine(classDir, dirName);
					Debug.WriteLine($"\t- Found '{Path}' device");
					var bMatch = true;
					foreach (var m in match)
					{

						var attribute = m.Key;
						var matches = m.Value;
						Debug.WriteLine($"\t\tTrying to find a matching attribute with '{attribute}' key. Values: {string.Join(", ", m.Value)}...");
						var strValue = GetAttrString(attribute);
						Debug.WriteLine($"\t\tAttribute value = '{strValue}'");


						if (matches.Any() && !string.IsNullOrEmpty(matches.First())
						                  && !matches.Any(x => x == strValue))
						{
							bMatch = false;
							break;
						}
					}

					if (bMatch)
					{
						Logger.Status(Status.OK, $"'{deviceType}' connected");
						return true;
					}

					Path = null;
				}
			}
			Logger.Status(Status.KO, $"'{deviceType}' disconnected!");
			return false;

		}

		public bool Connected => !string.IsNullOrEmpty(Path);

		public int DeviceIndex
		{
			get
			{
				AssertConnected();

				if (_deviceIndex < 0)
				{
					var f = 1;
					_deviceIndex = 0;
					foreach (var c in Path.Where(char.IsDigit))
					{
						_deviceIndex += (int)char.GetNumericValue(c) * f;
						f *= 10;
					}
				}

				return _deviceIndex;
			}

			protected set => _deviceIndex = value;
		}

		public int GetAttrInt(string name)
		{
			AssertConnected();

			using (var os = OpenStreamReader(name))
			{
				return int.Parse(os.ReadToEnd());
			}
		}

		public void SetAttrInt(string name, int value)
		{
			AssertConnected();

			using (var os = OpenStreamWriter(name))
			{
				os.Write(value);
			}
		}

		public string GetAttrString(string name)
		{
			AssertConnected();

			using (var os = OpenStreamReader(name))
			{
				return os.ReadToEnd().TrimEnd();
			}
		}

		public void SetAttrString(string name,
			string value)
		{
			AssertConnected();

			using (var os = OpenStreamWriter(name))
			{
				os.Write(value);
			}
		}

		public string GetAttrLine(string name)
		{
			AssertConnected();

			using (var os = OpenStreamReader(name))
			{
				return os.ReadLine();
			}
		}

		public string[] GetAttrSet(string name)
		{
			var s = GetAttrLine(name);
			return s.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
		}

		public string[] GetAttrSet(string name, out string pCur)
		{
			pCur = null;
			var result = GetAttrSet(name);
			var bracketedValue = result.FirstOrDefault(s => s.StartsWith("["));
			if (bracketedValue != null) 
				pCur = bracketedValue.Substring(1, bracketedValue.Length - 2);
			return result;
		}

		public string GetAttrFromSet(string name)
		{
			var result = GetAttrSet(name);
			var bracketedValue = result.FirstOrDefault(s => s.StartsWith("["));
			var pCur = bracketedValue?.Substring(1, bracketedValue.Length - 2);
			return pCur;
		}

		private StreamReader OpenStreamReader(string name)
		{
			return new StreamReader(new FileStream(global::System.IO.Path.Combine(Path, name), FileMode.Open, FileAccess.Read, FileShare.Read));
		}

		private StreamWriter OpenStreamWriter(string name)
		{
			return new StreamWriter(new FileStream(global::System.IO.Path.Combine(Path, name), FileMode.Create, FileAccess.Write, FileShare.Write));
		}

		private void AssertConnected()
		{
			if (!Connected)
				throw new InvalidOperationException("no device connected");
		}
	}
}