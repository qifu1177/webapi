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
    
    public class BaseRepositoryWithSessionForId<T> : BaseRepositoryWithSession<T>, IRepositoryWithSessionForId<T> where T : IdData
    {
        public BaseRepositoryWithSessionForId() : base()
        {            
        }
        public void DeleteWithId(IClientSessionHandle session, string id)
        {
            _collection.DeleteOne(session,data => data.Id == id);
        }

        public T LoadWithId(IClientSessionHandle session, string id)
        {
            IEnumerable<T> list = this.Load(session,data => data.Id == id);
            if (list.Count() > 0)
                return list.First();
            return null;
        }

        public void UpdateWithId(IClientSessionHandle session, string id, T data)
        {
            this.Update(session,data, item => item.Id == id);
        }

        IRepositoryWithSessionForId<T> IRepositoryWithSessionForId<T>.Init(IMongoDatabase db)
        {
            _db = db;
            _collection = _db.GetCollection<T>(typeof(T).Name);
            return this;
        }
    }
}
