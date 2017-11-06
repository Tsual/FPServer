namespace FPServer.Interfaces
{
    public interface IEncryptor
    {
        string Decrypt(string metaStr);
        string Encrypt(string metaStr);
    }
}