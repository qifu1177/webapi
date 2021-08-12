using Autofac;
using Data.DataAccess;
using Data.Interfaces;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data
{
    public class Module : Autofac.Module
    {
        private string _connectionStr;
        private string _dbName;
        public Module(string connectionStr, string dbName)
        {
            _connectionStr = connectionStr;
            _dbName = dbName;
        }
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<MongoClient>().As<IMongoClient>().WithParameter("connectionString", _connectionStr).InstancePerLifetimeScope();
            builder.Register(c =>c.Resolve<IMongoClient>().GetDatabase(_dbName)).As<IMongoDatabase>().InstancePerLifetimeScope();
            builder.RegisterGeneric(typeof(BaseRepository<>)).As(typeof(IRepository<>)).InstancePerLifetimeScope();
            builder.RegisterGeneric(typeof(BaseRepositoryForId<>)).As(typeof(IRepositoryForId<>)).InstancePerLifetimeScope();
            builder.RegisterGeneric(typeof(BaseRepositoryWithSession<>)).As(typeof(IRepositoryWithSession<>)).InstancePerLifetimeScope();
            builder.RegisterGeneric(typeof(BaseRepositoryWithSessionForId<>)).As(typeof(IRepositoryWithSessionForId<>)).InstancePerLifetimeScope();
            builder.RegisterType<BaseWorkOfUnit>().As<IWorkOfUnit>().InstancePerLifetimeScope();
            builder.RegisterType<UserWorkOfUnit>().As<IUserWorkOfUnit>().InstancePerLifetimeScope();
        }
    }
}
