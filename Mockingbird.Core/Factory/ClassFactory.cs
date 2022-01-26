using Mockingbird.Output;
using System.Reflection;

namespace Mockingbird.Factory
{
    public class ClassFactory : IObjectFactory
    {
        public bool TryCreateInstance(Type type, IObjectFactoryContext context, out object? instance)
        {
            ConstructorInfo[] constructors = type.GetConstructors();
            foreach (ConstructorInfo constructor in constructors)
            {
                try
                {
                    //parameters
                    ParameterInfo[] parameterInfos = constructor.GetParameters();
                    (bool success, object? inst)[] parameters = parameterInfos
                        .Select(parameterInfo =>
                        {
                            bool success = context.RootFactory.TryCreateInstance(parameterInfo.ParameterType, context, out object? inst);
                            return (success, inst);
                        })
                        .ToArray();
                    if (parameters.All(p => p.success))
                    {
                        instance = constructor.Invoke(parameters.Select(p => p.inst).ToArray());
                        context.LogOutput.InstanceCreated(type, nameof(ClassFactory));
                        return true;
                    }
                }
                catch (Exception ex)
                {
                    context.LogOutput.Invoke($"Tried to create instance {type.FullName} by {nameof(ClassFactory)} failed because: {ex.Message}");
                    // ignore
                }
            }

            context.LogOutput.Invoke($"Could not create instance {type.FullName} by {nameof(ClassFactory)}");
            instance = null;
            return false;
        }
    }
}
