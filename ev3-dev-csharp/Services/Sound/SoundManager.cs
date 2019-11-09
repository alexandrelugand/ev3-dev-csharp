using System.Diagnostics;
using log4net;

namespace EV3.Dev.Csharp.Services.Sound
{
	public class SoundManager : ISoundManager
	{
		private readonly ILog _logger;
		private Process _process;
		private int _volume;

		public SoundManager(ILog logger)
		{
			_logger = logger;
			Volume = 50;
		}

		public void InterruptSound()
		{
			if (_process != null && !_process.HasExited)
			{
				_process.EnableRaisingEvents = false;
				_process.Kill();
				_process = null;
				_logger.Debug("Interrupt sound!");
			}
		}

		public void PlaySoundFile(string soundFilePath)
		{
			InterruptSound();
			_process = new Process
			{
				EnableRaisingEvents = true,
				StartInfo =
				{
					FileName = "aplay",
					Arguments = soundFilePath
				}
			};
			_process.Start();
			_process.Exited += (sender, args) => _logger.Debug($"'{soundFilePath}' sound finished");
			_logger.Debug($"Play '{soundFilePath}' sound...");
		}

		public int Volume
		{
			get => _volume;
			set
			{
				_volume = value;
				SetVolume(_volume);
			}
		}

		private void SetVolume(int volume)
		{
			var p = new Process
			{
				StartInfo =
				{
					FileName = "amixer",
					Arguments = $"set PCM {volume}%"
				}
			};
			p.Start();
		}
	}
}
