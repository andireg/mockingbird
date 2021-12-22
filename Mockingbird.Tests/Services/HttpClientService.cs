using System.Collections.Generic;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace Mockingbird.Tests.Services
{
    internal class HttpClientService
    {
        private readonly HttpClient httpClient;

        public HttpClientService(HttpClient httpClient)
        {
            this.httpClient = httpClient;
        }

        internal async Task<string> SendGetRequestAsync(string input, CancellationToken cancellationToken)
        {
            HttpResponseMessage? message = await httpClient.GetAsync($"https://google.com?q={input}", cancellationToken);
            if (message.IsSuccessStatusCode)
            {
                return await message.Content.ReadAsStringAsync(cancellationToken);
            }

            return "NULL";
        }

        internal async Task<string> SendPostRequestAsync(Dictionary<string, string> input, CancellationToken cancellationToken)
        {
            FormUrlEncodedContent? content = new(input);
            HttpResponseMessage? message = await httpClient.PostAsync($"https://google.com", content, cancellationToken);
            if (message.IsSuccessStatusCode)
            {
                return await message.Content.ReadAsStringAsync(cancellationToken);
            }

            return "NULL";
        }
    }
}