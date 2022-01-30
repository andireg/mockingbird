using System.Reflection;

namespace Mockingbird.Factory
{
    public class ClassFactory : IObjectFactory
    {
        public bool CanCreateInstance(Type type, IObjectFactoryContext context)
            => type.IsClass && !type.IsAbstract;

        public object CreateInstance(Type type, IObjectFactoryContext context)
        {
            ConstructorInfo[] constructors = type.GetConstructors();
            foreach (ConstructorInfo constructor in constructors)
            {
                try
                {
                    ParameterInfo[] parameterInfos = constructor.GetParameters();
                    object?[] parameters = parameterInfos
                        .Select(parameterInfo => context.RootFactory.CreateInstance(parameterInfo.ParameterType, context))
                        .ToArray();
                    return constructor.Invoke(parameters);
                }
                catch (Exception ex)
                {
                    context.LogOutput.Invoke($"Tried to create instance {type.FullName} by {nameof(ClassFactory)} failed because: {ex.Message}");
                    // ignore
                }
            }

            return new NotSupportedException($"Could not create instance of {type.FullName}");
        }
    }
}