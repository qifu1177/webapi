using System;
using System.Collections.Generic;
using System.Text;

namespace BLL
{
    public abstract class AbstractLogic<T> : Singleton<T>
    {
        protected string _connectStr = "";
        public T SetConnectString(string connectStr)
        {
            _connectStr = connectStr;
            return Instance;
        }
        
    }
}
