using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Data.Interfaces
{
    public interface IRepository<T> where T:class
    {
        IRepository<T> Init(IMongoDatabase db);
        IEnumerable<T> LoadAll();
        IEnumerable<T> Load(Expression<Func<T, bool>> filter);
        T Insert(T data);
        void Update(T data, Expression<Func<T, bool>> filter);
        
        void Delete(Expression<Func<T, bool>> filter);
        
    }
}
