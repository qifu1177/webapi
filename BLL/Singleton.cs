using System;
using System.Collections.Generic;
using System.Text;

namespace BLL
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
    public class Test : Singleton<Test>
    {
        public void DoSomething(int n, string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                throw new ArgumentException("Name is blank.", "name");
            }
            //...
        }
    }
    
}
