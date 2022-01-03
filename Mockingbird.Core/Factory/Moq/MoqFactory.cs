using Mockingbird.Utils;
using Mockingbird.Invocation;
using Moq;
using System.Linq.Expressions;
using System.Reflection;

namespace Mockingbird.Factory.Moq
{
    internal class MoqFactory : IObjectFactory
    {
        private const string SetupMethodName = "Setup";
        private static readonly Type MockType = typeof(Mock<>);
        private static readonly Type VoidType = typeof(void);
        private static readonly Type TaskType = typeof(Task);
        private static readonly MethodInfo TaskFromResultMethodInfo = TaskType.GetMethodInfo("FromResult<TResult>(TResult)");
        private static readonly IEnumerable<Type> ignoreTypes = new[] { typeof(CancellationToken) };

        public bool TryCreateInstance(Type type, IObjectFactoryContext context, out object? instance)
        {
            if (type == null)
            {
                throw new ArgumentNullException(nameof(type));
            }

            if (!type.IsInterface && !type.IsAbstract)
            {
                instance = null;
                return false;
            }

            Type mockType = MockType.MakeGenericType(type);
            Mock moqMock = (Mock)Activator.CreateInstance(mockType)!;

            if (context.SetupProvider.TryGetSetup(type, out TypeInvocationInfo? typeSetup)) 
            { 
                moqMock = SetupMock(moqMock, context, type, typeSetup!);
            }

            instance = moqMock.Object;

            ITypeInvocationProvider typeInvocationProvider = context.InvocationProvider.ForType(type);
            context.InvocationProvider.BeforeCollectInvocations(() => 
            {
                foreach (IInvocation? invocation in moqMock.Invocations)
                {
                    ParameterInfo[] parameters = invocation.Method.GetParameters();
                    Dictionary<string, object> args = new();
                    for (int i = 0; i < parameters.Length; i++)
                    {
                        args.Add(
                            parameters[i].Name!,
                            ignoreTypes.Contains(parameters[i].ParameterType) ? "*" : invocation.Arguments[i]);
                    }

                    InvocationInfo? setupInfo = typeSetup?.GetInvocation(invocation.Method.Name, args);
                    typeInvocationProvider.AddInvocation(
                        invocation.Method.Name,
                        args,
                        setupInfo?.Result);
                }
            });

            return true;
        }

        private static Mock SetupMock(
            Mock mock,
            IObjectFactoryContext context,
            Type serviceType,
            TypeInvocationInfo typeSetup)
        {
            foreach (InvocationInfo invocationInfo in typeSetup.Invocations)
            {
                SetupFunction(
                    mock,
                    context,
                    serviceType,
                    invocationInfo);
            }

            return mock;
        }

        private static void SetupFunction(
            Mock mock,
            IObjectFactoryContext context,
            Type serviceType,
            InvocationInfo snapshotFunction)
        {
            Dictionary<string, object> setupArgs = 
                ObjectConverter.ConvertObject<Dictionary<string, object>>(snapshotFunction.Arguments) ??
                new Dictionary<string, object>();
            MethodInfo instanceMethodInfo = serviceType.GetMethodInfo(
                snapshotFunction.InvocationName,
                setupArgs.Select(prop => prop.Key));
            Expression setupExpression = ExpressionUtils.CreateSetupExpression(
                serviceType,
                instanceMethodInfo,
                setupArgs);

            object returnSetup;
            if (instanceMethodInfo.ReturnType == VoidType)
            {
                returnSetup = mock.GetType().GetMethods()
                    .Single(methodInfo => methodInfo.Name == SetupMethodName && !methodInfo.IsGenericMethod)
                    .Invoke(mock, new object[] { setupExpression })!;
            }
            else
            {
                returnSetup = mock.GetType().GetMethods()
                    .Single(methodInfo => methodInfo.Name == SetupMethodName && methodInfo.IsGenericMethod)
                    .MakeGenericMethod(instanceMethodInfo.ReturnType)
                    .Invoke(mock, new object[] { setupExpression })!;
            }

            if (snapshotFunction.Result != null)
            {
                Type responseType = instanceMethodInfo.ReturnType.GetRootType();
                if (responseType == VoidType)
                {
                    return;
                }

                object? value = null;
                if (snapshotFunction.Result.ToString() == "<MOCK>")
                {
                    if (context.RootFactory.TryCreateInstance(responseType, context, out object? val))
                    {
                        value = val;
                    }
                }
                else
                {
                    value = ConvertUtils.ConvertToType(snapshotFunction.Result, responseType);
                    bool isAsync = instanceMethodInfo.ReturnType.IsTaskType();
                    if (isAsync)
                    {
                        value = MakeTaskValue(value!, responseType);
                    }
                }

                MethodInfo returnMethodInfo = returnSetup.GetType().GetMethodInfo($"Returns({instanceMethodInfo.ReturnType.GetTypeName()})");
                returnMethodInfo.Invoke(returnSetup, value == null ? Array.Empty<object>() : new object[] { value! });
            }
        }

        private static object MakeTaskValue(object value, Type type)
        {
            MethodInfo fromResultMethodInfo = TaskFromResultMethodInfo.MakeGenericMethod(type);
            return fromResultMethodInfo.Invoke(null, new object[] { value })!;
        }
    }
}
