using Data.Interfaces;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Data.DataAccess
{
   

    public class BaseRepositoryWithSession<T> : IRepositoryWithSession<T> where T : class
    {
        protected IMongoDatabase _db;
        protected IMongoCollection<T> _collection;
        public BaseRepositoryWithSession()
        {           
        }

        public void Delete(IClientSessionHandle session, Expression<Func<T, bool>> filter)
        {
            _collection.DeleteMany(session,filter);
        }

        public IRepositoryWithSession<T> Init(IMongoDatabase db)
        {
            _db = db;
            _collection = _db.GetCollection<T>(typeof(T).Name);
            return this;
        }

        public T Insert(IClientSessionHandle session, T data)
        {
            _collection.InsertOne(session,data);
            return data;
        }

        public IEnumerable<T> Load(IClientSessionHandle session, Expression<Func<T, bool>> filter)
        {
            return _collection.Find<T>(session,filter).ToList();
        }

        public IEnumerable<T> LoadAll(IClientSessionHandle session)
        {
            return this.Load(session,t => true);
        }

        public void Update(IClientSessionHandle session, T data, Expression<Func<T, bool>> filter)
        {
            _collection.ReplaceOne(session,filter, data);

        }


    }
}
