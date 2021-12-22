using Mockingbird.Utils;
using Mockingbird.Invocation;
using Newtonsoft.Json;
using System.Net.Http.Json;

namespace Mockingbird.Factory.Http
{
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
                    Content = await request.Content.ReadAsStringAsync(cancellationToken),
                    ContentType = request.Content.Headers?.ContentType?.MediaType,
                };
            InvocationInfo? functionSetup = typeSetup?.Invocations?.FirstOrDefault(fnc =>
                fnc.InvocationName == invocationName &&
                JsonConvert.SerializeObject(fnc.Arguments ?? string.Empty) == JsonConvert.SerializeObject(invocationArgument ?? string.Empty));
            HttpResponse? httpResponse = ObjectConverter.ConvertObject<HttpResponse>(functionSetup?.Result);
            if (httpResponse != null)
            {
                httpResponseMessage.StatusCode = (System.Net.HttpStatusCode)httpResponse.Status;
                switch (httpResponse.ContentType ?? string.Empty)
                {
                    case "application/json":
                        httpResponseMessage.Content = httpResponse.Content == null ? null : JsonContent.Create(JsonConvert.DeserializeObject(httpResponse.Content));
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
                    Content = httpResponseMessage.Content == null ? null : await httpResponseMessage.Content.ReadAsStringAsync(cancellationToken),
                    ContentType = httpResponseMessage.Content?.Headers?.ContentType?.MediaType,
                });

            return httpResponseMessage;
        }
    }
}