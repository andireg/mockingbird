using System.Text.Json;
using Mockingbird.Invocation;

namespace Mockingbird.Setup;

internal class SetupProvider : ISetupProvider
{
    private readonly SetupProviderOptions options;
    private readonly IEnumerable<TypeInvocationInfo> typeInvocationInfos;
    private static readonly JsonSerializerOptions? jsonSerializerOptions = new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        WriteIndented = true,
        DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull,
        PropertyNameCaseInsensitive = true,
        DictionaryKeyPolicy = JsonNamingPolicy.CamelCase,
    };

    public SetupProvider(SetupProviderOptions options)
    {
        this.options = options;
        if (File.Exists(options.SetupFile))
        {
            string json = File.ReadAllText(options.SetupFile);
            typeInvocationInfos = 
                JsonSerializer.Deserialize<TypeInvocationInfo[]>(json, jsonSerializerOptions) ?? 
                Array.Empty<TypeInvocationInfo>();
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
                        JsonSerializer.Serialize(fnc.Arguments ?? string.Empty) == JsonSerializer.Serialize(setupInvocation.Arguments ?? string.Empty));

                    int usedNumber = usedInvocation?.Number ?? 0;

                    if (usedNumber != setupInvocation.Number)
                    {
                        options.LogOutput?.Invoke($"Expexted invocation: {setupInvocation.InvocationName} {JsonSerializer.Serialize(setupInvocation.Arguments ?? string.Empty)}");
                        if (usedTypeInvocation?.Invocations != null)
                        {
                            foreach (InvocationInfo? invocationInfo in usedTypeInvocation.Invocations.Where(inv => inv.InvocationName == setupInvocation.InvocationName))
                            {
                                options.LogOutput?.Invoke($"Called invocations: {invocationInfo?.InvocationName} {JsonSerializer.Serialize(invocationInfo?.Arguments ?? string.Empty)}");
                            }
                        }

                        throw Xunit.Sdk.EqualException.ForMismatchedValues(
                            setupInvocation.Number,
                            usedNumber,
                            $"Number of invocations do not match: {setupInvocation.InvocationName} {JsonSerializer.Serialize(setupInvocation.Arguments ?? string.Empty)}");
                    }
                }
            }
        }

        if (!File.Exists(options.SetupFile))
        {
            string json = JsonSerializer.Serialize(usedTypeInvocations, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(options.SetupFile, json);
        }
    }
}