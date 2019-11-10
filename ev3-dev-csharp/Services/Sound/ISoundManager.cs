namespace EV3.Dev.Csharp.Services.Sound
{
	public interface ISoundManager
	{
		void InterruptSound();
		void PlaySound(string soundFilePath);
		int Volume { get; set; }
	}
}