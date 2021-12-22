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
                        return true;
                    }
                }
                catch (Exception)
                {
                    // ignore
                }
            }
            instance = null;
            return false;
        }
    }
}
