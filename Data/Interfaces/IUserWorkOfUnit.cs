using Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Interfaces
{
    public interface IUserWorkOfUnit
    {
        AppUser LoadUserWithId(string id);
        AppUser LoadUserWithUn(string userName, string password);
        AppUser LoadUserWithEmail(string email, string password);
        AppUser LoadUserWithSessionId(string sessionId);
        IEnumerable<AppUser> LoadAllUser();
        IEnumerable<AppUser> LoadUsersWithParentId(string accountUserId);
        void UpdateAppUser(AppUser user);
        Dictionary<string, string> LoadModuleRights(string roleId);
        string LoadRoleId(string role);
        void InserUser(string userName, string password, string email, string roleId);
        bool EmailIsExist(string email);
        bool UserNameIsExist(string userName);
        void DeleteUserWithId(string id);
        void InserUser(AppUser user);
    }
}
