using Autofac;
using Data.Interfaces;
using Data.Models;
using Domain.Models;
using Help;
using Help.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain
{
    public class DbInit : Singleton<DbInit>
    {
        private string _connectionStr;
        private string _dbName;
        private IContainer _container;
        public DbInit Init(string connectionStr, string dbName)
        {
            _connectionStr = connectionStr;
            _dbName = dbName;
            if (_container == null)
            {
                var builder = new ContainerBuilder();
                builder.RegisterModule(new Module(connectionStr, dbName));
                _container = builder.Build();
            }

            return this;
        }

        public void InitRole()
        {
            var values = Enum.GetValues(typeof(EnumData.Role));
            using (var scope = _container.BeginLifetimeScope())
            {
                var work = scope.Resolve<IWorkOfUnit>();
                var repository = scope.Resolve<IRepository<AppRole>>();
                var moduleRepository = scope.Resolve<IRepository<AppModule>>();
               
                work.Run((db) =>
                {
                    IEnumerable<AppRole> existsList = repository.Init(db).LoadAll();
                    IEnumerable<AppModule> modulesList = moduleRepository.Init(db).LoadAll();
                    foreach (var value in values)
                    {
                        //repository.Init(db).Delete(item => item.Name == value.ToString());
                        if (existsList.Where(role => role.Name == value.ToString()).Count() == 0)
                        {
                            AppRole role = new AppRole { Name = value.ToString(), ModuleRights = new Dictionary<string, string>() };
                            if(role.Name==EnumData.Role.Admin.ToString())
                            {
                                foreach(var module in modulesList)
                                {
                                    role.ModuleRights.Add(module.Name, EnumData.RoleRight.all.ToString());
                                }
                            }
                            if (role.Name == EnumData.Role.Manager.ToString())
                            {
                                foreach (var module in modulesList)
                                {
                                    if (module.Name == "AppRole")
                                        continue;
                                    role.ModuleRights.Add(module.Name, EnumData.RoleRight.all.ToString());
                                }
                            }
                            if (role.Name == EnumData.Role.User.ToString())
                            {
                                foreach (var module in modulesList)
                                {
                                    if (module.Name == "AppRole" || module.Name== "AppUser")
                                        continue;
                                    role.ModuleRights.Add(module.Name, EnumData.RoleRight.read.ToString());
                                }
                            }
                            repository.Insert( role);
                        }
                            
                    }
                });
            }
        }

        public void InitModules()
        {
            List<AppModule> list = new List<AppModule>();
            list.Add(new AppModule { Name = "AppUser" });
            list.Add(new AppModule { Name = "AppRole" });
            list.Add(new AppModule { Name = "House" });
            list.Add(new AppModule { Name = "Thing" });
            list.Add(new AppModule { Name = "GridThings" });
            using (var scope = _container.BeginLifetimeScope())
            {
                var work = scope.Resolve<IWorkOfUnit>();
                var repository = scope.Resolve<IRepository<AppModule>>();

                work.Run((db) =>
                {
                    IEnumerable<AppModule> existsList = repository.Init(db).LoadAll();
                    foreach (var item in list)
                    {
                        if (existsList.Where(module => module.Name == item.Name).Count() == 0)
                            repository.Insert(item);
                    }
                });
            }
        }
       
        public void InitAdminUser(dynamic user)
        {
            using (var scope = _container.BeginLifetimeScope())
            {
                var work = scope.Resolve<IWorkOfUnit>();
                var repository = scope.Resolve<IRepository<AppUser>>();
                var roleRepository = scope.Resolve<IRepository<AppRole>>();
                work.Run((db) =>
                {
                    DateTime currentTs = DateTime.UtcNow;
                    string email = user.Email;
                    IEnumerable<AppRole> roles = roleRepository.Init(db).Load(item => item.Name == EnumData.Role.Admin.ToString());
                    IEnumerable<AppUser> users = repository.Init(db).Load((item) => item.Email == email);
                    //repository.Init(db).Delete(item => item.Email == email);
                    if (users.Count() == 0 && roles.Count()>0)
                    {
                        AppUser adminUser = new AppUser { Name = user.Name, Email = user.Email,RoleId=roles.First().Id, Password = MD5HashService.Instance.CreateMD5Hash(user.Password), CreateTs = currentTs, UpdateTs = currentTs };
                        repository.Init(db).Insert(adminUser);
                    }
                });
            }
        }

    }
}
