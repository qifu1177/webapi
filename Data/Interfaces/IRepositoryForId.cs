using Data.Abstracts;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Interfaces
{
    public interface IRepositoryForId<T>:IRepository<T> where T:IdData
    {
        new IRepositoryForId<T> Init(IMongoDatabase db);
        T LoadWithId(string id);
        void UpdateWithId(string id, T data);
        void DeleteWithId(string id);
    }
}
