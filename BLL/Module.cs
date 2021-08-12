
using Autofac;

namespace BLL
{
    public class Module:Autofac.Module
    {
        private string _dbConnectionString;
        public Module(string dbConnectionString)
        {
            _dbConnectionString = dbConnectionString;
        }
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterModule(new Dal.Module(_dbConnectionString));
        }
    }
}
