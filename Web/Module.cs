using Autofac;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace Web
{
    public class Module : Autofac.Module
    {
        private string _dbConnetctionString;
        private string _dbName;
        public Module(string dbConnetctionString,string dbName)
        {
            _dbConnetctionString = dbConnetctionString;
            _dbName = dbName;
        }
        protected override void Load(ContainerBuilder builder)
        {
            string nameSpace = this.GetType().Namespace;
            builder.RegisterModule(new Help.Module(Assembly.GetExecutingAssembly(), $"{nameSpace}.Resources.translation.json"));
            builder.RegisterModule(new Domain.Module(_dbConnetctionString,_dbName));
        }
    }
}
