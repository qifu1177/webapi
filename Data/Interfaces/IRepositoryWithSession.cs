using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Data.Interfaces
{
    public interface IRepositoryWithSession<T> where T:class
    {
        IRepositoryWithSession<T> Init(IMongoDatabase db);
        IEnumerable<T> LoadAll(IClientSessionHandle session);
        IEnumerable<T> Load(IClientSessionHandle session,Expression<Func<T, bool>> filter);
        T Insert(IClientSessionHandle session,T data);
        void Update(IClientSessionHandle session,T data, Expression<Func<T, bool>> filter);

        void Delete(IClientSessionHandle session,Expression<Func<T, bool>> filter);
    }
}
