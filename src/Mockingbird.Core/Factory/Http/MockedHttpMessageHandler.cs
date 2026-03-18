using Mockingbird.Invocation;
using Mockingbird.Utils;
using System.Net.Http.Json;
using System.Text.Json;

namespace Mockingbird.Factory.Http;

internal class MockedHttpMessageHandler : HttpMessageHandler
{
    private readonly TypeInvocationInfo? typeSetup;
    private readonly ITypeInvocationProvider typeInvocationProvider;

    public MockedHttpMessageHandler(
        TypeInvocationInfo? typeSetup,
        ITypeInvocationProvider typeInvocationProvider)
    {
        this.typeSetup = typeSetup;
        this.typeInvocationProvider = typeInvocationProvider;
    }

    protected override async Task<HttpResponseMessage> SendAsync(
        HttpRequestMessage request,
        CancellationToken cancellationToken)
    {
        HttpResponseMessage httpResponseMessage = new();
        string invocationName = $"{request.Method} {request.RequestUri}";
        object? invocationArgument = request.Content == null ?
            null :
            new
            {
                Content = await request.Content.ReadAsStringAsync(),
                ContentType = request.Content.Headers?.ContentType?.MediaType,
            };
        InvocationInfo? functionSetup = typeSetup?.Invocations?.FirstOrDefault(fnc =>
            fnc.InvocationName == invocationName &&
            JsonSerializer.Serialize(fnc.Arguments ?? string.Empty) == JsonSerializer.Serialize(invocationArgument ?? string.Empty));
        HttpResponse? httpResponse = ObjectConverter.ConvertObject<HttpResponse>(functionSetup?.Result);
        if (httpResponse != null)
        {
            httpResponseMessage.StatusCode = (System.Net.HttpStatusCode)httpResponse.Status;
            switch (httpResponse.ContentType ?? string.Empty)
            {
                case "application/json":
                    // TODO
                    /*
                    httpResponseMessage.Content = httpResponse.Content == null ? null : JsonContent.Create(
                        JsonSerializer.Deserialize(httpResponse.Content));
                        */
                    break;

                default:
                    break;
            }
        }
        else
        {
            httpResponseMessage.StatusCode = System.Net.HttpStatusCode.OK;
            httpResponseMessage.Content = JsonContent.Create(true);
        }

        typeInvocationProvider.AddInvocation(
            invocationName,
            invocationArgument,
            new HttpResponse
            {
                Status = (int)httpResponseMessage.StatusCode,
                Content = httpResponseMessage.Content == null ? null : await httpResponseMessage.Content.ReadAsStringAsync(),
                ContentType = httpResponseMessage.Content?.Headers?.ContentType?.MediaType,
            });

        return httpResponseMessage;
    }
}