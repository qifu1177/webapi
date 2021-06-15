using BLL.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace BLL
{
    public class UserLogic: AbstractLogic<UserLogic>
    {
        private string CreateSession(Dal.Models.DataContext context,string userId)
        {
            string sessionId = Guid.NewGuid().ToString();
            Dal.Models.AppSession session = new Dal.Models.AppSession();
            session.SessionId = sessionId;
            session.UserId = userId;
            context.AppSessions.Add(session);
            context.SaveChanges();
            return sessionId;
        }
        public string Login(string email,string password)
        {
            string backStr = "";
            using(Dal.Models.DataContext context =new Dal.Models.DataContext(_connectStr))
            {
                var user=context.AppUsers.Where(item => item.Email == email && item.Password == password).First();
                if (user == null)
                {
                    backStr = ConstentValue.USER_LOGIN_EMAIL_OR_PASSWORD_ERROR;
                }
                else
                {
                    var query = context.Modules.Join(context.RoleRights
                        , module => module.ModuleId
                        , roleRight => roleRight.ModuleId
                        , (module, roleRight) => new { MonduleName = module.ModuleName, Right = roleRight.RoleRight1, RoleId = roleRight.RoleId })
                        .Join(context.AppUsers
                        , moduleRight => moduleRight.RoleId
                        , user => user.RoleId
                        , (moduleRight, user) => new
                        {
                            MonduleName = moduleRight.MonduleName,
                            Right = moduleRight.Right,
                            UserId = user.UserId
                        }).Where(moduleUser => moduleUser.UserId == user.UserId);
                    List<dynamic> list = new List<dynamic>();
                    foreach (var moduleUser in query)
                    {
                        dynamic item = new ExpandoObject();
                        item.module = moduleUser.MonduleName;
                        item.right = moduleUser.Right;
                        list.Add(item);                        
                    }
                    dynamic resulte = new ExpandoObject();
                    resulte.message = "OK";
                    resulte.sessionId = CreateSession(context, user.UserId);
                    resulte.loginTs = DateTime.UtcNow.ToJsTime();
                    resulte.modules= list;               
                    backStr = Newtonsoft.Json.JsonConvert.SerializeObject(resulte, Formatting.None);
                }
            }
            return backStr;
        }

        private Dal.Models.AppUser CreateUser(Dictionary<string, string> registerData,string roleId)
        {
            Dal.Models.AppUser user = new Dal.Models.AppUser();
            user.UserId = Guid.NewGuid().ToString();
            foreach(string key in registerData.Keys)
            {
                if (key == "userName")
                    user.UserName = registerData[key];
                else if (key == "email")
                    user.Email = registerData[key];
                else if (key == "psw")
                    user.Password = registerData[key];
            }
            user.RoleId = roleId;
            user.CreateTs = DateTime.UtcNow;
            user.UpdateTs = user.CreateTs;
            return user;
        }
        private bool Validate(string email)
        {
            bool b = Regex.IsMatch(email, ConstentValue.EMAIL_PATTERN);
            if(b)
            {
                using (Dal.Models.DataContext context = new Dal.Models.DataContext(_connectStr))
                {
                    var user = context.AppUsers.Single(item => item.Email == email);
                    b = user == null;

                }
            }            
            return b;
        }
        private string GetRoleId(string roleDesc)
        {
            string backStr = "";
            using(Dal.Models.DataContext context=new Dal.Models.DataContext(_connectStr))
            {
                var role = context.AppRoles.Single(item => item.RoleDesc == roleDesc);
                if (role != null)
                    backStr = role.RoleId;
            }
            return backStr;
        }
        public string Register(Dictionary<string,string> registerData)
        {
            string backStr = "";
            if(registerData.ContainsKey("email"))
            {
                string roleId = GetRoleId(EnumData.Role.Manager.ToString());
                if(string.IsNullOrEmpty(roleId))
                {
                    return ConstentValue.USER_REGISTER_ROLE_ERROR;
                }
                if (Validate(registerData["email"]))
                {                    
                    Dal.Models.AppUser user = CreateUser(registerData,roleId);
                    using(Dal.Models.DataContext context=new Dal.Models.DataContext(_connectStr))
                    {
                        context.AppUsers.Add(user);
                        context.SaveChanges();
                    }
                }
                else
                {
                    backStr = ConstentValue.USER_REGISTER_EMAIL_ERROR;
                }
            }
            return backStr;
        }
    }
}
