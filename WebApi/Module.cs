
using Autofac;
using System.Reflection;

namespace WebApi
{
    public class Module : Autofac.Module
    {
        private string _dbConnetctionString;
        public Module(string dbConnetctionString)
        {
            _dbConnetctionString = dbConnetctionString;
        }
        protected override void Load(ContainerBuilder builder)
        {
            string nameSpace = this.GetType().Namespace;
            //builder.RegisterModule(new Help.Module(Assembly.GetExecutingAssembly(), $"{nameSpace}.Resources.translation.json"));
            builder.RegisterModule(new BLL.Module(_dbConnetctionString));
        }
    }
}
