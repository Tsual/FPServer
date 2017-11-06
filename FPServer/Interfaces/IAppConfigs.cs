using FPServer.Enums;

namespace FPServer.Interfaces
{
    public interface IAppConfigs
    {
        string this[AppConfigEnum key] { get; set; }
    }
}