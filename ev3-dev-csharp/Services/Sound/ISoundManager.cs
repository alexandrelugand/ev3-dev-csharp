namespace EV3.Dev.Csharp.Services.Sound
{
    public interface ISoundManager
    {
        void Load();
        void InterruptSound();
        void PlaySound(string soundName);
        void PlaySoundAsync(string soundName);
        int Volume { get; set; }
    }
}