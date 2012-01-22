using System;
using System.Linq;
using System.Reflection;

namespace CityIndex.ReflectiveLoggingAdapter
{
    internal class ReflectedMethodInfo
    {
        public Type Type;
        public MethodInfo Method;
        public string MethodName;
        public Type[] ParameterTypes;
        public static ReflectedMethodInfo Create(Type type, string methodName, Type[] parameterTypes)
        {
            var m = new ReflectedMethodInfo(type, methodName, parameterTypes);
            if (m.Method != null)
            {
                return m;
            }
            return null;
        }


        private ReflectedMethodInfo(Type type, string methodName, Type[] parameterTypes)
        {
            Type = type;
            MethodName = methodName;
            ParameterTypes = parameterTypes;
            Method = type.GetMethod(methodName, BindingFlags.Public | BindingFlags.Instance, null, parameterTypes, null);
        }

        public static string GetToString(string methodName, params Type[] ptypes)
        {
            var p = string.Join(", ", ptypes.Select(t => t.Name).ToArray());
            string s = string.Format("{0} {1}", methodName, p);
            return s;
        }
        public override string ToString()
        {
            return GetToString(MethodName, ParameterTypes);
        }
    }
}