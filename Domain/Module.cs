using Autofac;
using Domain.Interfaces;
using Domain.Logics;
using Domain.Models.Requests;
using Domain.Models.Responses;
using Domain.Validators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain
{
    public class Module:Autofac.Module
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
            builder.RegisterModule(new Data.Module(_connectionStr, _dbName));
            builder.RegisterType<UserRegisterRequestValidator>().As<IValidatorWithTranslator<UserRegisterRequest>>().InstancePerLifetimeScope();
            builder.RegisterType<UserRequestValidator>().As<IValidatorWithTranslator<UserRequest>>().InstancePerLifetimeScope();
            builder.RegisterType<UserLogic>().As<IUserLogic>().InstancePerLifetimeScope();
            builder.RegisterType<FileLogic>().As<IFileLogic>().InstancePerLifetimeScope();
            builder.RegisterType<UserAdminLogic>().As<ILogic<UserRequest, UserListResponse>>().InstancePerLifetimeScope();
        }
    }
}
