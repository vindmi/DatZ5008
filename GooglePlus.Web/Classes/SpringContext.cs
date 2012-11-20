using System;
using System.Threading;
using Spring.Context;
using Spring.Context.Support;

namespace GooglePlus.Web.Classes
{
    public static class SpringContext
    {
        private static readonly Lazy<IApplicationContext> Context = 
            new Lazy<IApplicationContext>(ContextRegistry.GetContext, LazyThreadSafetyMode.ExecutionAndPublication);

        public static IApplicationContext Current
        {
            get { return Context.Value; }
        }

        public static object Resolve(string objectName)
        {
            return Current.GetObject(objectName);
        }

        public static T Resolve<T>()
        {
            return (T) Resolve(typeof(T).Name);
        }

        public static T Resolve<T>(string objectName)
        {
            return (T) Resolve(objectName);
        }
    }
}
