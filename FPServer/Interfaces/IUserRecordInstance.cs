using FPServer.Models;
using System.Collections;
using System.Collections.Generic;

namespace FPServer.Interfaces
{
    public interface IUserRecordInstance :IEnumerable
    {
        string this[string key] { get; set; }
        void Delete(string key);
        IList<UserRecordModel> GetAll();
    }
}