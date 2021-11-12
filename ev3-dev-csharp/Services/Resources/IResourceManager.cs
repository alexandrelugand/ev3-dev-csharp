namespace EV3.Dev.Csharp.Services.Resources
{
    public interface IResourceManager
    {
        void LoadSounds();
        string GetSounds(string soundName);
    }
}