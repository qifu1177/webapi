using System;
using System.Collections.Generic;
using System.Text;

namespace Help
{
    public abstract class Singleton<T>
    {
        private static T instance;
        public static T Instance
        {
            get
            {
                if (instance == null)
                {
                    Type type = typeof(T);
                    instance = (T)Activator.CreateInstance(type);
                }
                return instance;
            }
        }

    }
}
