namespace FPServer.Interfaces
{
    public interface IUserRecordInstance
    {
        string this[string key] { get; set; }
    }
}