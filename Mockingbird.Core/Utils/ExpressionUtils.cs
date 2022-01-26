using Moq;
using Newtonsoft.Json;
using System.Linq.Expressions;
using System.Reflection;

namespace Mockingbird.Factory.Moq
{
    internal static class ExpressionUtils
    {
        private static readonly Type ItType = typeof(It);
        private static readonly Type JsonConvertType = typeof(JsonConvert);
        private static readonly MethodInfo ItIsAnyMethodInfo = ItType.GetMethodInfo("IsAny<TValue>()");
        private static readonly MethodInfo ItIsMethodInfo = ItType.GetMethodInfo("Is<TValue>(Expression<Func<TValue, Boolean>>)");
        private static readonly MethodInfo JsonConvertSerializeObjectMethodInfo = JsonConvertType.GetMethodInfo("SerializeObject(Object)");

        internal static Expression CreateSetupExpression(
            Type serviceType,
            MethodInfo instanceMethodInfo,
            IDictionary<string, object> functionArguments)
        {
            ParameterExpression parameterExpression = Expression.Parameter(serviceType, "x");
            Expression[] parameterExpressions = instanceMethodInfo.GetParameters()
                .Select(parameter => CreateParameterExpression(parameter, functionArguments))
                .ToArray();
            Expression invocationExpression = Expression.Call(parameterExpression, instanceMethodInfo, parameterExpressions);
            return Expression.Lambda(invocationExpression, parameterExpression);
        }

        internal static Expression CreateParameterExpression(
            ParameterInfo parameterInfo,
            IDictionary<string, object> setupArguments)
        {
            object? setupValue = setupArguments.ContainsKey(parameterInfo.Name!) ?
                setupArguments[parameterInfo.Name!] :
                null;
            bool isAny = setupValue?.ToString() == "*";
            if (isAny)
            {
                return ExpressionUtils.CreateIsAnyExpression(parameterInfo.ParameterType);
            }
            else
            {
                return ExpressionUtils.CreateIsExpression(parameterInfo.ParameterType, setupValue);
            }
        }

        internal static Expression CreateIsAnyExpression(Type parameterType)
        {
            MethodInfo typedIsAnyMethodInfo = ItIsAnyMethodInfo.MakeGenericMethod(parameterType);
            return Expression.Call(null, typedIsAnyMethodInfo);
        }

        internal static Expression CreateIsExpression(
            Type parameterType,
            object? referenceValue)
        {
            ParameterExpression parameter = Expression.Parameter(parameterType, "x");
            Expression referenceValueExpression;
            Expression currentValueExpression;
            if (!parameterType.IsPrimitive && parameterType != typeof(string))
            {
                referenceValueExpression = Expression.Constant(referenceValue == null ? null : JsonConvert.SerializeObject(referenceValue!));
                currentValueExpression = Expression.Call(JsonConvertSerializeObjectMethodInfo, Expression.Convert(parameter, typeof(object)));
            }
            else
            {
                try
                {
                    referenceValue = referenceValue == null ? null : ConvertUtils.ConvertToType(referenceValue!, parameterType);
                }
                catch
                {
                    // ignore casting issue
                }

                referenceValueExpression = Expression.Constant(referenceValue);
                currentValueExpression = parameter;
            }

            BinaryExpression equalExpression = Expression.Equal(currentValueExpression, referenceValueExpression);
            Expression lamdaExpression = Expression.Lambda(equalExpression, parameter);
            MethodInfo isMethodInfo = ItIsMethodInfo.MakeGenericMethod(parameterType);
            return Expression.Call(null, isMethodInfo, lamdaExpression);
        }
    }
}