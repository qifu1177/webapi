using Data.Abstracts;
using Data.Interfaces;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.DataAccess
{
    public class BaseRepositoryForId<T> : BaseRepository<T>, IRepositoryForId<T> where T : IdData
    {
        public BaseRepositoryForId():base()
        {            
        }
        public void DeleteWithId(string id)
        {
            _collection.DeleteOne(data => data.Id == id);
        }      

        public T LoadWithId(string id)
        {
            IEnumerable<T> list = this.Load(data => data.Id == id);
            if (list.Count() > 0)
                return list.First();
            return null;
        }

        public void UpdateWithId(string id, T data)
        {
            this.Update(data, item => item.Id == id);
        }

        IRepositoryForId<T> IRepositoryForId<T>.Init(IMongoDatabase db)
        {
            _db = db;
            _collection = _db.GetCollection<T>(typeof(T).Name);
            return this;
        }
    }
}
