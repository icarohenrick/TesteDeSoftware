using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Mime;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace NerdStore.WebApp.Tests
{
    public static class ClientExtensions
    {
        public static async Task<HttpResponseMessage> PostAsJsonAsync<TModel>(this HttpClient client, string requestUrl, TModel model)
        {
            var json = JsonSerializer.Serialize(model);
            var stringContent = new StringContent(json, Encoding.UTF8, MediaTypeNames.Application.Json);
            return await client.PostAsync(requestUrl, stringContent);
        }

        public static async Task<HttpResponseMessage> PostAsJsonAsync<TModel>(this HttpClient client, string requestUrl, TModel model, string token)
        {
            var json = JsonSerializer.Serialize(model);
            var stringContent = new StringContent(json, Encoding.UTF8, MediaTypeNames.Application.Json);

            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            
            return await client.PostAsync(requestUrl, stringContent);
        }

        public static async Task<HttpResponseMessage> DeleteAsync(this HttpClient client, string requestUrl, string token)
        {
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            return await client.DeleteAsync(requestUrl);
        }
    }
}