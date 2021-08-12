using System;
using System.Dynamic;

namespace DbInit
{
    public class Program
    {
        public static void Main(string[] args)
        {
          
            dynamic parameter = InitParamenter(args);
            if(string.IsNullOrEmpty(parameter.connecStr))
            {
                Console.WriteLine("Connectingstring is empty.");
                return;
            }           
            
            if (parameter.initModules)
                Domain.DbInit.Instance.Init(parameter.connecStr, parameter.dbName).InitModules();
            if (parameter.initRole)
                Domain.DbInit.Instance.Init(parameter.connecStr, parameter.dbName).InitRole();
            if (parameter.addAdmin)
                Domain.DbInit.Instance.Init(parameter.connecStr, parameter.dbName).InitAdminUser(parameter.user);

        }

        private static dynamic InitParamenter(string[] args)
        {
            string connecStr = "";
            string dbName = "";
            bool initRole = true;
            bool initModules = true;
            bool addAdmin = false;
            string email = "";
            string psw = "";
            string un = "";
            foreach(string s in args)
            {
                if(s.StartsWith("ConnectionString") || s.StartsWith("constr"))
                {
                    connecStr = s.Substring(s.IndexOf('=') + 1);
                }
                if (s.StartsWith("DBName") )
                {
                    dbName = s.Substring(s.IndexOf('=') + 1);
                }
                if (s.StartsWith("InitModules"))
                {
                    string[] strs = s.Split('=');
                    if (strs.Length > 1)
                        initModules = Convert.ToBoolean(strs[1]);
                }
                if (s.StartsWith("InitRole"))
                {
                    string[] strs = s.Split('=');
                    if (strs.Length > 1)
                        initRole = Convert.ToBoolean(strs[1]);
                }
                if (s.StartsWith("Email"))
                {
                    string[] strs = s.Split('=');
                    if (strs.Length > 1)
                        email = strs[1];
                }
                if (s.StartsWith("password") || s.StartsWith("psw"))
                {
                    string[] strs = s.Split('=');
                    if (strs.Length > 1)
                        psw = strs[1];
                }
                if (s.StartsWith("username") || s.StartsWith("un"))
                {
                    string[] strs = s.Split('=');
                    if (strs.Length > 1)
                        un = strs[1];
                }
            }
            if(!string.IsNullOrEmpty(email) && !string.IsNullOrEmpty(psw) )
            {
                addAdmin = true;
                if (string.IsNullOrEmpty(un))
                    un = email;
            }
            dynamic user = new ExpandoObject();
            user.Email = email;
            user.Password = psw;
            user.Name = un;
            return new
            {
                connecStr = connecStr,
                dbName = dbName,
                initRole = initRole,
                initModules=initModules,
                addAdmin = addAdmin,
                user = user,
            };
        }

        
    }
}
