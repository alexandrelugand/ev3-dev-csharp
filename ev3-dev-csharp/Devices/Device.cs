using EV3.Dev.Csharp.Core.Helpers;
using EV3.Dev.Csharp.Services;
using log4net;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;

namespace EV3.Dev.Csharp.Devices
{
    public class Device : IDisposable
    {
        protected const string SysRoot = "/sys/";
        protected readonly ILog Logger;
        private int _deviceIndex = -1;
        private bool _disposed;

        public Device()
        {
            Logger = Ev3.Instance.Resolve<ILog>();
        }

        protected string Path { get; set; }

        protected bool Connect(string deviceType, string classDir, string pattern, IDictionary<string, string[]> match)
        {
            if (!Directory.Exists(classDir))
                return false;

            Logger.Debug($"##### > Trying to connect device '{classDir}' with '{pattern}' pattern ...");
            var dirs = Directory.EnumerateDirectories(classDir);
            foreach (var currentFullDirPath in dirs)
            {
                var dirName = System.IO.Path.GetFileName(currentFullDirPath);
                if (dirName != null && dirName.StartsWith(pattern))
                {
                    Path = System.IO.Path.Combine(classDir, dirName);
                    Logger.Debug($"\t- Found '{Path}' device");
                    var bMatch = true;
                    foreach (var m in match)
                    {
                        var attribute = m.Key;
                        var matches = m.Value;
                        Logger.Debug($"\t\tTrying to find a matching attribute with '{attribute}' key. Values: {string.Join(", ", m.Value)}...");
                        var strValue = GetAttrString(attribute);
                        Logger.Debug($"\t\tAttribute value = '{strValue}'");


                        if (matches.Any() && !string.IsNullOrEmpty(matches.First())
                                          && matches.All(x => x != strValue))
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
                try
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
                }
                catch (Exception e)
                {
                    Logger.Error("Failed to ge device index", e);
                }

                return _deviceIndex;
            }

            protected set => _deviceIndex = value;
        }

        public int GetAttrInt(string name)
        {
            try
            {
                AssertConnected();
                using (var os = OpenStreamReader(name))
                {
                    return int.Parse(os.ReadToEnd());
                }
            }
            catch (Exception e)
            {
                Logger.Error("Failed to get device index", e);
                return default;
            }
            finally
            {
                Thread.Sleep(10);
            }
        }

        public void SetAttrInt(string name, int value, int count = 0)
        {
            if (count > 3)
                return;

            try
            {
                AssertConnected();
                using (var os = OpenStreamWriter(name))
                {
                    os.Write(value);
                }
            }
            catch (IOException)
            {
                Thread.Sleep(100);
                SetAttrInt(name, value, ++count);
            }
            catch (Exception e)
            {
                Logger.Error("Failed to set attribute Int", e);
            }
        }

        public string GetAttrString(string name)
        {
            try
            {
                AssertConnected();
                using (var os = OpenStreamReader(name))
                {
                    return os.ReadToEnd().TrimEnd();
                }
            }
            catch (Exception e)
            {
                Logger.Error("Failed to get attribute String", e);
                return default;
            }
        }

        public void SetAttrString(string name, string value, int count = 0)
        {
            if (count > 3)
                return;

            try
            {
                AssertConnected();
                using (var os = OpenStreamWriter(name))
                {
                    os.Write(value);
                }
            }
            catch (IOException)
            {
                Thread.Sleep(100);
                SetAttrString(name, value, ++count);
            }
            catch (Exception e)
            {
                Logger.Error("Failed to set attribute String", e);
            }
        }

        public string GetAttrLine(string name)
        {
            try
            {
                AssertConnected();
                using (var os = OpenStreamReader(name))
                {
                    return os.ReadLine();
                }
            }
            catch (Exception e)
            {
                Logger.Error("Failed to get attribute line", e);
                return default;
            }
        }

        public string[] GetAttrSet(string name)
        {
            try
            {
                var s = GetAttrLine(name);
                return s.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            }
            catch (Exception e)
            {
                Logger.Error("Failed to get attribute set", e);
                return default;
            }
        }

        public string[] GetAttrSet(string name, out string pCur)
        {
            pCur = null;

            try
            {
                var result = GetAttrSet(name);
                var bracketedValue = result.FirstOrDefault(s => s.StartsWith("["));
                if (bracketedValue != null)
                    pCur = bracketedValue.Substring(1, bracketedValue.Length - 2);
                return result;
            }
            catch (Exception e)
            {
                Logger.Error("Failed to get attribute set", e);
                return default;
            }
        }

        public string GetAttrFromSet(string name)
        {
            try
            {
                var result = GetAttrSet(name);
                var bracketedValue = result.FirstOrDefault(s => s.StartsWith("["));
                var pCur = bracketedValue?.Substring(1, bracketedValue.Length - 2);
                return pCur;
            }
            catch (Exception e)
            {
                Logger.Error("Failed to get attribute from set", e);
                return default;
            }
        }

        private StreamReader OpenStreamReader(string name) => new StreamReader(new FileStream(global::System.IO.Path.Combine(Path, name), FileMode.Open, FileAccess.Read, FileShare.ReadWrite));

        private StreamWriter OpenStreamWriter(string name) => new StreamWriter(new FileStream(global::System.IO.Path.Combine(Path, name), FileMode.OpenOrCreate, FileAccess.Write, FileShare.Write));

        private void AssertConnected()
        {
            if (!Connected)
                throw new InvalidOperationException("no device connected");
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                _disposed = disposing;
            }
        }

        public void Dispose()
        {
            Dispose(true);
        }
    }
}
