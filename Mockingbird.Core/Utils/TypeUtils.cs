using Microsoft.CSharp;
using System.CodeDom;
using System.Reflection;

namespace Mockingbird.Utils
{
    internal static class TypeUtils
    {
        private static readonly Dictionary<string, Type> friendyTypes = new();
        internal static readonly Type VoidType = typeof(void);
        internal static readonly Type TaskType = typeof(Task);
        internal static readonly Type TaskGenericType = typeof(Task<>);

        static TypeUtils()
        {
            Assembly mscorlib = Assembly.GetAssembly(typeof(int))!;
            using CSharpCodeProvider provider = new();
            foreach (TypeInfo? type in mscorlib.DefinedTypes)
            {
                if (string.Equals(type.Namespace, "System"))
                {
                    CodeTypeReference? typeRef = new(type);
                    string? csTypeName = provider.GetTypeOutput(typeRef);

                    if (!csTypeName.Contains('.'))
                    {
                        friendyTypes.Add(csTypeName, type);
                    }
                }
            }
        }

        public static Type? GetTypeByFriendlyName(string name)
        {
            return friendyTypes.ContainsKey(name) ? friendyTypes[name] : Type.GetType($"System.{name}");
        }

        public static Type GetRootType(this Type type)
        {
            if (type == null)
            {
                throw new ArgumentNullException(nameof(type));
            }

            if (type == TaskType ||
                type == VoidType)
            {
                return VoidType;
            }

            if (type.IsGenericType &&
                type.GetGenericTypeDefinition() == TaskGenericType)
            {
                return type.GenericTypeArguments[0];
            }

            return type;
        }

        public static bool IsTaskType(this Type type)
        {
            if (type == null)
            {
                throw new ArgumentNullException(nameof(type));
            }

            if (TaskType.IsAssignableFrom(type))
            {
                return true;
            }

            if (type.IsGenericType && type.GetGenericTypeDefinition() == TaskGenericType)
            {
                return true;
            }

            return false;
        }
    }
}
