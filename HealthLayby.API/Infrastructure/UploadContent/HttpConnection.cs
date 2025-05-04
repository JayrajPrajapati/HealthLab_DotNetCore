using System.Net.Http.Headers;
using System.Net;

namespace HealthLayby.API.Infrastructure.UploadContent
{
    public static class HttpConnection
    {
        public static HttpClient CreateClient(string baseURL,string secretKey)
        {
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;
            HttpClient httpClient = new HttpClient();
            httpClient.BaseAddress = new Uri(baseURL);
            httpClient.DefaultRequestHeaders.Clear();
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            httpClient.DefaultRequestHeaders.Add("cache-control", "no-cache");
            //httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", secretKey);
            httpClient.DefaultRequestHeaders.Add("Authorization", secretKey);
            return httpClient;
        }
    }
}
