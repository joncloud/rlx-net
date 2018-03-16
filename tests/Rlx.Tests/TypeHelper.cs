using System;
using static Rlx.Functions;

namespace Rlx.Tests
{
    static class TypeHelper
    {
        public static Type GetType(string messageTypeName) =>
            GetTypeFromExecutingAssembly(messageTypeName)
                .OrElse(() => GetTypeFromLoadedAssemblies(messageTypeName))
                .Unwrap();

        static Option<Type> GetTypeFromExecutingAssembly(string typeName) =>
            Type.GetType(typeName, throwOnError: false).ToOption();

        static Option<Type> GetTypeFromLoadedAssemblies(string typeName)
        {
            foreach (var a in AppDomain.CurrentDomain.GetAssemblies())
            {
                var type = a.GetType(typeName);
                if (type != null)
                    return Some(type);
            }
            return None<Type>();
        }
    }
}
