using BLL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace BLL
{
    public class DbInit : AbstractLogic<DbInit>
    {

        public void InitRole()
        {
            using (Dal.Models.DataContext context = new Dal.Models.DataContext(_connectStr))
            {
                var values = Enum.GetValues(typeof(EnumData.Role));
                bool isAdded = false;
                foreach (var value in values)
                {
                    string roleStr = value.ToString();
                    var roles = context.AppRoles.Where(item => item.RoleDesc == roleStr).ToList();
                    if (roles.Count <= 0)
                    {
                        Dal.Models.AppRole role = new Dal.Models.AppRole();
                        role.RoleId = Guid.NewGuid().ToString();
                        role.RoleDesc = value.ToString();
                        context.AppRoles.Add(role);
                        isAdded = true;
                    }
                }
                if (isAdded)
                    context.SaveChanges();
            }

        }

        public void InitModules()
        {
            using(Dal.Models.DataContext context=new Dal.Models.DataContext(_connectStr))
            {
                Type type = typeof(Dal.Models.DataContext);
                var properties = type.GetProperties();
                bool isAdded = false;
                foreach(var property in properties)
                {
                    if(property.PropertyType.Name.StartsWith("DbSet") )
                    {
                        if (property.Name == "Modules" || property.Name == "AppSessions")
                            continue;
                        int count = context.Modules.Where(item => item.ModuleName == property.Name).Count();
                        if(count<=0)
                        {
                            Dal.Models.Module module = new Dal.Models.Module();
                            module.ModuleName = property.Name;
                            context.Modules.Add(module);
                            isAdded = true;
                        }
                    }
                }
                if (isAdded)
                    context.SaveChanges();
            }
        }
        public void InitAdminModulesRight()
        {
            using(Dal.Models.DataContext context=new Dal.Models.DataContext(_connectStr))
            {
                var adminRole = context.AppRoles.Where(item => item.RoleDesc == EnumData.Role.Admin.ToString()).ToArray();
                if(adminRole.Length>0)
                {
                    var modules = context.Modules.ToArray();
                    bool isAdded = false;
                    List<int> addedModules = context.RoleRights.Where(item => item.RoleId == adminRole[0].RoleId).Select(item => item.ModuleId).ToList();
                    foreach(var module in modules)
                    {
                        if (addedModules.Contains(module.ModuleId))
                            continue;
                        Dal.Models.RoleRight roleRight = new Dal.Models.RoleRight();
                        roleRight.RoleId = adminRole[0].RoleId;
                        roleRight.ModuleId = module.ModuleId;
                        roleRight.RoleRight1 = EnumData.RoleRight.a.ToString();
                        context.RoleRights.Add(roleRight);
                        isAdded = true;
                    }
                    if (isAdded)
                        context.SaveChanges();
                }
            }
        }
        public string CreateMD5Hash(string input)
        {
            MD5 md5 = System.Security.Cryptography.MD5.Create();
            byte[] inputBytes = System.Text.Encoding.ASCII.GetBytes(input);
            byte[] hashBytes = md5.ComputeHash(inputBytes);


            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < hashBytes.Length; i++)
            {
                sb.Append(hashBytes[i].ToString("x2"));
            }
            return sb.ToString();
        }
        public void InitAdminUser(dynamic user)
        {
            using (Dal.Models.DataContext context = new Dal.Models.DataContext(_connectStr))
            {
                var role = context.AppRoles.Single(item => item.RoleDesc == EnumData.Role.Admin.ToString());
                string email = user.Email;
                var admins = context.AppUsers.Where(item => item.Email == email).ToList();
                if (admins.Count <= 0)
                {
                    Dal.Models.AppUser admin = new Dal.Models.AppUser();
                    admin.RoleId = role.RoleId;
                    admin.Email = email;
                    admin.Password = CreateMD5Hash(user.Password);
                    admin.UserName = user.Name;
                    admin.UserId = Guid.NewGuid().ToString();
                    admin.CreateTs = DateTime.UtcNow;
                    admin.UpdateTs = admin.CreateTs;
                    context.AppUsers.Add(admin);
                    context.SaveChanges();
                }
                else
                {
                    throw new Exception("The email has already been registered.");
                }
            }
        }
    }
}
