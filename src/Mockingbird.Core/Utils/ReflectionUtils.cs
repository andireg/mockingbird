using System.Reflection;
using System.Text.RegularExpressions;

namespace Mockingbird.Factory.Moq
{
    internal static class ReflectionUtils
    {
        internal static MethodInfo GetMethodInfo(this Type type, string methodName, IEnumerable<string> parameterNames)
        {
            MethodInfo? methodInfo = GetMethodInfoInternal(type, methodName, parameterNames);
            if (methodInfo == null)
            {
                throw new Exception($"In class {type.FullName}, no method {methodName}({string.Join(", ", parameterNames)}) found.");
            }

            return methodInfo;
        }

        private static MethodInfo? GetMethodInfoInternal(Type type, string methodName, IEnumerable<string> parameterNames)
        {
            MethodInfo? methodInfo = Array.Find(
                    type.GetMethods(),
                    mi => mi.Name == methodName &&
                        string.Equals(
                            string.Join(",", mi.GetParameters().Select(parameter => parameter.Name)),
                            string.Join(",", parameterNames)));
            if (methodInfo != null)
            {
                return methodInfo;
            }

            foreach (Type interfaceType in type.GetInterfaces())
            {
                methodInfo = GetMethodInfoInternal(interfaceType, methodName, parameterNames);
                if (methodInfo != null)
                {
                    return methodInfo;
                }
            }

            return null;
        }

        internal static MethodInfo GetMethodInfo(this Type type, string method)
        {
            Match match = Regex.Match(method, @"^([^\(<]*)<{0,1}([^>]*){0,1}>{0,1}\(([^\)]*)\)$");
            if (!match.Success)
            {
                throw new Exception($"No method info {method} found in type {type.FullName}");
            }

            string methodName = match.Groups[1].Value;
            bool isGeneric = !string.IsNullOrEmpty(match.Groups[2].Value);
            string parameterTypes = match.Groups[3].Value;
            MethodInfo[] methodInfos = type.GetMethods()
                .Where(methodInfo =>
                    methodInfo.Name == methodName &&
                    methodInfo.IsGenericMethod == isGeneric &&
                    parameterTypes == string.Join(",", methodInfo.GetParameters().Select(param => param.ParameterType.GetTypeName())))
                .ToArray();

            return methodInfos.Length switch
            {
                1 => methodInfos[0],
                0 => throw new Exception($"No method info {method} found in type {type.FullName}"),
                _ => throw new Exception($"Method info {method} not unique in type {type.FullName}"),
            };
        }

        internal static string GetTypeName(this Type paramterType)
        {
            string name = paramterType.Name;
            if (paramterType.IsGenericType)
            {
                name = name[..name.LastIndexOf("`")];
                return $"{name}<{string.Join(", ", paramterType.GetGenericArguments().Select(GetTypeName))}>";
            }

            return name;
        }
    }
}