using Autofac;
using Help.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Help
{
    public class Module : Autofac.Module
    {
        private Assembly _assembly;
        private string _namespaceAndFileName;
        public Module(Assembly assembly, string namespaceAndFileName)
        {
            _assembly = assembly;
            _namespaceAndFileName = namespaceAndFileName;
        }
        protected override void Load(ContainerBuilder builder)
        {
            Translator t = new Translator(_assembly, _namespaceAndFileName);
            builder.RegisterInstance(t).As<ITranslator>().ExternallyOwned();
        }
    }
}
