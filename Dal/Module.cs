using Autofac;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dal
{
    public class Module: Autofac.Module
    {
        private string _dbConnectionString;
        public Module(string dbConnectionString)
        {
            _dbConnectionString = dbConnectionString;
        }
        protected override void Load(ContainerBuilder builder)
        {
            
        }
    }
}
