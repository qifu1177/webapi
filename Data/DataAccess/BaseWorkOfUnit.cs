using Data.Interfaces;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.DataAccess
{
    public class BaseWorkOfUnit : IWorkOfUnit
    {
        protected IMongoDatabase _db;
        public BaseWorkOfUnit(IMongoDatabase db)
        {
            _db = db;            
        }

        public IMongoDatabase DB => _db;

        public void Run(Action<IMongoDatabase> action)
        {
            try
            {
                action(_db);                
            }
            catch (Exception ex)
            {                
                throw;
            }
        }

        public void Transaction(Action<IMongoDatabase,IClientSessionHandle> action)
        {
            using (var session = _db.Client.StartSession())
            {                
                session.StartTransaction();
                try
                {
                    action(_db,session);
                    session.CommitTransaction();
                }
                catch (Exception ex)
                {
                    session.AbortTransaction();
                    throw;
                }
            }
        }
    }
}
