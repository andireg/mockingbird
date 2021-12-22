using Mockingbird.Invocation;
using Newtonsoft.Json;

namespace Mockingbird.Setup
{
    internal class SetupProvider : ISetupProvider
    {
        private readonly string setupFile;
        private readonly IEnumerable<TypeInvocationInfo> typeInvocationInfos;

        public SetupProvider(string setupFile)
        {
            this.setupFile = setupFile;
            if (File.Exists(setupFile))
            {
                string json = File.ReadAllText(setupFile);
                typeInvocationInfos = JsonConvert.DeserializeObject<TypeInvocationInfo[]>(json) ?? Array.Empty<TypeInvocationInfo>();
            }

            if (typeInvocationInfos == null)
            {
                typeInvocationInfos = Array.Empty<TypeInvocationInfo>();
            }
        }

        public bool TryGetSetup(Type type, out TypeInvocationInfo? typeSetup)
        {
            typeSetup = typeInvocationInfos.FirstOrDefault(tp => tp.TypeName == type.FullName);
            return typeSetup != null;
        }

        public void Verify(IEnumerable<TypeInvocationInfo> usedTypeInvocations)
        {
            foreach (TypeInvocationInfo setupType in typeInvocationInfos)
            {
                TypeInvocationInfo? usedTypeInvocation = usedTypeInvocations.FirstOrDefault(ti => ti.TypeName == setupType.TypeName);
                foreach (InvocationInfo setupInvocation in setupType.Invocations)
                {
                    if (setupInvocation.Number > 0)
                    {
                        InvocationInfo? usedInvocation = usedTypeInvocation?.Invocations?.FirstOrDefault(fnc =>
                            fnc.InvocationName == setupInvocation.InvocationName &&
                            JsonConvert.SerializeObject(fnc.Arguments ?? string.Empty) == JsonConvert.SerializeObject(setupInvocation.Arguments ?? string.Empty));

                        int usedNumber = usedInvocation?.Number ?? 0;

                        if(usedNumber != setupInvocation.Number)
                        {
                            throw new Xunit.Sdk.AssertActualExpectedException(
                                setupInvocation.Number,
                                usedNumber, 
                                $"Number of invocations do not match: {setupInvocation.InvocationName} {JsonConvert.SerializeObject(setupInvocation.Arguments ?? string.Empty)}");
                        }
                    }
                }
            }

            if (!File.Exists(setupFile))
            {
                string json = JsonConvert.SerializeObject(usedTypeInvocations, Formatting.Indented);
                File.WriteAllText(setupFile, json);
            }
        }
    }
}
