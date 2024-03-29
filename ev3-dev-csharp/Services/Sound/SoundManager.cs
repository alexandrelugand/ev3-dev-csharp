﻿using EV3.Dev.Csharp.Services.Resources;
using log4net;
using System.Diagnostics;

namespace EV3.Dev.Csharp.Services.Sound
{
    public class SoundManager : ISoundManager
    {
        private readonly ILog _logger;
        private readonly IResourceManager _resourceManager;
        private Process _process;
        private int _volume;

        public SoundManager(ILog logger, IResourceManager resourceManager)
        {
            _logger = logger;
            _resourceManager = resourceManager;
            Volume = 50;
        }

        public void Load()
        {
            _resourceManager.LoadSounds();
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

        public void PlaySound(string soundName)
        {
            PlaySoundAsync(soundName);
            _process?.WaitForExit();
        }

        public void PlaySoundAsync(string soundName)
        {
            InterruptSound();

            var sound = _resourceManager.GetSounds(soundName);
            if (string.IsNullOrEmpty(sound))
                return;

            _process = new Process
            {
                EnableRaisingEvents = true,
                StartInfo =
                {
                    FileName = "aplay",
                    Arguments = $"--buffer-size=16384 -q {sound}"
                }
            };
            _process.Start();
            _process.Exited += (sender, args) => _logger.Debug($"'{soundName}' sound finished");
            _logger.Debug($"Play '{soundName}' sound...");
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
                    Arguments = $"-q set PCM {volume}%"
                }
            };
            p.Start();
        }
    }
}
