using Autofac;
using Help.Constents;
using Help.Datas;
using Help.Interfaces;
using Help.Services;
using Microsoft.Extensions.Configuration;
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
        private IConfiguration _configuration;
        public Module(Assembly assembly, string namespaceAndFileName, IConfiguration configuration)
        {
            _assembly = assembly;
            _namespaceAndFileName = namespaceAndFileName;
            _configuration = configuration;
        }
        protected override void Load(ContainerBuilder builder)
        {
            Translator t = new Translator(_assembly, _namespaceAndFileName);
            builder.RegisterInstance(t).As<ITranslator>().ExternallyOwned();
            UpdateAppSetting();
            UpdateUploadFileTypes(AppSetting.Instance.UploadFileTypes);
            builder.RegisterInstance(AppSetting.Instance).As<IAppSetting>().ExternallyOwned();
            builder.RegisterType<EmailService>().As<IEmailService>().InstancePerLifetimeScope();
            builder.RegisterType<PasswordService>().As<IPasswordService>().InstancePerLifetimeScope();
        }
        private void UpdateAppSetting()
        {
            AppSetting.Instance.SessionDuration = Convert.ToInt32(_configuration[ConstentConfigKey.Session_Duration]);
            AppSetting.Instance.UploadFileMaxSize = Convert.ToInt32(_configuration[ConstentConfigKey.UploadFile_MaxSize]);
            AppSetting.Instance.EmailHost = _configuration[ConstentConfigKey.EmailServer_Host];
            AppSetting.Instance.EmailPort = Convert.ToInt32(_configuration[ConstentConfigKey.EmailServer_Port]);
            AppSetting.Instance.SenderAccount = _configuration[ConstentConfigKey.EmailServer_From];
            AppSetting.Instance.SenderAccountPassword = _configuration[ConstentConfigKey.EmailServer_FromPassword];
        }
        private void UpdateUploadFileTypes(List<string> list)
        {
            for (int i = 0; ; i++)
            {
                string vs = _configuration[$"{ConstentConfigKey.UploadFile_Type}:{i}"];
                if (string.IsNullOrEmpty(vs))
                    break;
                list.Add(vs);
            }
        }
    }
}
