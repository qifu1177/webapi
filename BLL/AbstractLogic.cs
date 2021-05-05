using System;
using System.Collections.Generic;
using System.Text;

namespace BLL
{
    public abstract class AbstractLogic<T> : Singleton<T>
    {
        internal string CopyFile(string sessionId,string fileName)
        {
            string str = string.Format("{0}/{1}",sessionId,fileName);
            return str;
        }
    }
}
