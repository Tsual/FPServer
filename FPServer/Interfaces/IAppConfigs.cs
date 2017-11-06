namespace FPServer.Interfaces
{
    public interface IAppConfigs
    {
        string this[string key] { get; set; }
    }
}