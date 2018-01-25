using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Challenge.SDK.RequestInputs;

namespace Challenge.SDK.Client
{
    public class ApiHost
    {
        public string Address { get; set; }
        public string Endpoint { get; set; }
    }

    public class WriteTextFileLib
    {
        public WriteTextFileLib(string address, string endpoint)
        {
            ApiHost = new ApiHost
            {
                Address = address,
                Endpoint = endpoint
            };
        }

        public ApiHost ApiHost { get; set; }

        public async Task<string> WriteFile(string exchange, string routeKey, string message)
        {
            HttpClient httpClient = new HttpClient();

            httpClient.BaseAddress = new Uri($"http://{this.ApiHost.Address}");

            DefaultContractResolver contractResolver = new DefaultContractResolver
            {
                NamingStrategy = new CamelCaseNamingStrategy()
            };

            var serializedBody = JsonConvert.SerializeObject(new TextRequest() { Text = message }, Formatting.None, new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore,
                ContractResolver = contractResolver
            });

            var body = new StringContent(serializedBody, Encoding.UTF8, "application/json");
            var requestMessage = new HttpRequestMessage(HttpMethod.Post, ApiHost.Endpoint) { Content = body };
            requestMessage.Headers.Add("exchange", new[] { exchange });
            requestMessage.Headers.Add("route-key", new[] { routeKey });

            var response = await httpClient.SendAsync(requestMessage);

            response.EnsureSuccessStatusCode();

            return response.Headers.Location.ToString();
        }
    }
}