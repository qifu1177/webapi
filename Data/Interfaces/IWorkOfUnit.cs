using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Interfaces
{
    public interface IWorkOfUnit
    {
        IMongoDatabase DB { get; }
        void Run(Action<IMongoDatabase> action);
        void Transaction(Action<IMongoDatabase,IClientSessionHandle> action);
        
    }
}
