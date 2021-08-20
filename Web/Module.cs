using Autofac;
using Help.Constents;
using Microsoft.Extensions.Configuration;
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
        private IConfiguration _configuration;
        public Module(IConfiguration configuration)
        {
            _configuration = configuration;
            _dbConnetctionString = _configuration[ConstentConfigKey.MongoDB_ConnectionStr]; 
            _dbName = _configuration[ConstentConfigKey.MongoDB_DBName];
        }
        protected override void Load(ContainerBuilder builder)
        {
            string nameSpace = this.GetType().Namespace;
            builder.RegisterModule(new Help.Module(Assembly.GetExecutingAssembly(), $"{nameSpace}.Resources.translation.json",_configuration));
            builder.RegisterModule(new Domain.Module(_dbConnetctionString,_dbName));
        }
    }
}
