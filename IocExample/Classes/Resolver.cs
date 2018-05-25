using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IocExample.Classes
{
    class Resolver
    {
        private static Dictionary<Type, Type> dictionaryTypes
            = new Dictionary<Type, Type>();

        private static Dictionary<Type, object> dictionaryObjects
            = new Dictionary<Type, object>();

        public static void Bind<T>(object obj)
        {
            dictionaryObjects.Add(typeof(T), obj);
        }



        public static void Bind<T,V>() where V : T
        {
            dictionaryTypes.Add(typeof(T), typeof(V));
        }



        public static T Get<T>()
        {           
            return (T)Get(typeof(T));
        }



        private static object Get(Type type)
        {
            if (dictionaryObjects.ContainsKey(type))
            {
                return dictionaryObjects[type];
            }
            else
            {
                type = dictionaryTypes[type];
                var constructor = Utils.GetSingleConstructor(type);                

                List<Object> resolvedParameters = new List<object>();

                foreach (var parameter in constructor.GetParameters())
                {
                    resolvedParameters.Add(Get(parameter.ParameterType));
                }

                return Utils.CreateInstance(type, resolvedParameters);
            }
        }
    }
}
