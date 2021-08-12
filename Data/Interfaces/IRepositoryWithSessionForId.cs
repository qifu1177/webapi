using Data.Abstracts;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Interfaces
{
    
    public interface IRepositoryWithSessionForId<T> : IRepositoryWithSession<T> where T : IdData
    {
        new IRepositoryWithSessionForId<T> Init(IMongoDatabase db);
        T LoadWithId(IClientSessionHandle session,string id);
        void UpdateWithId(IClientSessionHandle session,string id, T data);
        void DeleteWithId(IClientSessionHandle session,string id);
    }
}
