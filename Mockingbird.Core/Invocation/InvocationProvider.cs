namespace Mockingbird.Invocation
{
    internal class InvocationProvider : IInvocationProvider
    {
        private readonly IDictionary<string, TypeInvocationProvider> typeProviders = new Dictionary<string, TypeInvocationProvider>();
        private readonly List<Action> beforeCollectInvocations = new();

        public void BeforeCollectInvocations(Action callback)
            => beforeCollectInvocations.Add(callback);

        public ITypeInvocationProvider ForType(Type type)
        {
            string key = type.FullName!;
            if (!typeProviders.ContainsKey(key))
            {
                typeProviders.Add(key, new TypeInvocationProvider(type));
            }

            return typeProviders[key];
        }

        public IEnumerable<TypeInvocationInfo> GetInvocations()
        {
            foreach (Action callback in beforeCollectInvocations)
            {
                callback.Invoke();
            }

            return typeProviders
                  .Select(p => p.Value.GetInvocations())
                  .OrderBy(i => i.TypeName)
                  .ToArray();
        }
    }
}
