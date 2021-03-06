using Data.Interfaces;
using Data.Models;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.DataAccess
{
    public class UserWorkOfUnit : BaseWorkOfUnit, IUserWorkOfUnit
    {
        private IRepositoryForId<AppUser> _repository;
        private IRepositoryForId<AppRole> _roleRepository;
        public UserWorkOfUnit(IMongoDatabase db, IRepositoryForId<AppUser> userRepository, IRepositoryForId<AppRole> roleRepository) : base(db)
        {
            _db = db;
            _repository = userRepository.Init(_db);
            _roleRepository = roleRepository.Init(_db);
        }
        public AppUser LoadUserWithEmail(string email)
        {
            IEnumerable<AppUser> list = _repository.Load(item => item.Email.ToLower() == email.ToLower() );
            if (list.Count() > 0)
                return list.First();
            return null;
        }

        public AppUser LoadUserWithUn(string userName)
        {
            IEnumerable<AppUser> list = _repository.Load(item => item.Name == userName);
            if (list.Count() > 0)
                return list.First();
            return null;
        }
        public Dictionary<string, string> LoadModuleRights(string roleId)
        {
            AppRole role = _roleRepository.LoadWithId(roleId);
            return role.ModuleRights;
        }

        public void UpdateAppUser(AppUser user)
        {
            _repository.UpdateWithId(user.Id, user);
        }

        public string LoadRoleId(string role)
        {
            IEnumerable<AppRole> list = _roleRepository.Load(item => item.Name == role);
            if (list.Count() > 0)
                return list.First().Id;
            return null;
        }

        public void InserUser(string userName, string password, string email, string roleId)
        {
            _repository.Insert(new AppUser
            {
                Name = userName,
                Password = password,
                Email = email,
                RoleId = roleId
            });
        }

        public bool EmailIsExist(string email)
        {
            return _repository.Load(item => item.Email.ToLower() == email.ToLower()).Count() > 0;
        }

        public bool UserNameIsExist(string userName)
        {
            return _repository.Load(item => item.Name == userName).Count() > 0;
        }

        public AppUser LoadUserWithSessionId(string sessionId)
        {
            IEnumerable<AppUser> list = _repository.Load(item => item.Session.Id == sessionId);
            if (list.Count() > 0)
                return list.First();
            return null;
        }

        public void DeleteUserWithId(string id)
        {
            _repository.DeleteWithId(id);
        }

        public void InserUser(AppUser user)
        {
            _repository.Insert(user);
        }

        public AppUser LoadUserWithId(string id)
        {
            return _repository.LoadWithId(id);
        }

        

        public IEnumerable<AppUser> LoadAllUser()
        {
            return _repository.LoadAll();
        }

        public IEnumerable<AppUser> LoadUsersWithParentId(string accountUserId)
        {
            return _repository.Load(item => item.AccountUserId == accountUserId);
        }
    }
}
