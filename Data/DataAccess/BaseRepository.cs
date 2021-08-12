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
    public class BaseRepository<T> : IRepository<T> where T : class
    {       
        protected IMongoDatabase _db;
        protected IMongoCollection<T> _collection;
        public BaseRepository()
        {            
        }
       
        public void Delete(Expression<Func<T, bool>> filter)
        {
            _collection.DeleteMany(filter);
        }

        public IRepository<T> Init(IMongoDatabase db)
        {
            _db = db;
            _collection = _db.GetCollection<T>(typeof(T).Name);
            return this;
        }

        public T Insert(T data)
        {
            _collection.InsertOne(data);
            return data;
        }

        public IEnumerable<T> Load(Expression<Func<T, bool>> filter)
        {
            return _collection.Find<T>(filter).ToList();
        }

        public IEnumerable<T> LoadAll()
        {
            return this.Load(t => true);
        }

        public void Update(T data, Expression<Func<T, bool>> filter)
        {
            _collection.ReplaceOne(filter, data);
           
        }

        
    }
}
