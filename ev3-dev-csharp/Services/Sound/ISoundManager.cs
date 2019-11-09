namespace EV3.Dev.Csharp.Services.Sound
{
	public interface ISoundManager
	{
		void InterruptSound();
		void PlaySoundFile(string soundFilePath);
		int Volume { get; set; }
	}
}