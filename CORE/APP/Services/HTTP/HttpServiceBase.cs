using CORE.APP.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
using System.Net.Http.Json;

namespace CORE.APP.Services.HTTP
{
    // Provides a base class for HTTP service operations, including common methods for sending and receiving JSON data,
    // and handling authorization headers using the current HTTP context.

    public abstract class HttpServiceBase : ServiceBase
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IHttpClientFactory _httpClientFactory;

        protected HttpServiceBase(IHttpContextAccessor httpContextAccessor, IHttpClientFactory httpClientFactory)
        {
            _httpContextAccessor = httpContextAccessor;
            _httpClientFactory = httpClientFactory;
        }

        protected virtual HttpClient CreateHttpClient()
        {
            var httpClient = _httpClientFactory.CreateClient();
            var token = _httpContextAccessor.HttpContext?.Request.Headers["Authorization"].ToString();
            if (!string.IsNullOrEmpty(token))
            {
                if (token.StartsWith(JwtBearerDefaults.AuthenticationScheme))
                    token = token.Remove(0, JwtBearerDefaults.AuthenticationScheme.Length).TrimStart();
                httpClient.DefaultRequestHeaders.Add("Authorization", token);
            }
            return httpClient;
        }

        public virtual async Task<List<TResponse>> GetFromJson<TResponse>(string url, CancellationToken cancellationToken = default)
            where TResponse : Response, new()
        {
            var httpClient = CreateHttpClient();
            var list = await httpClient.GetFromJsonAsync<List<TResponse>>(url, cancellationToken);
            return list;
        }

        public virtual async Task<TResponse> GetFromJson<TResponse>(string url, int id, CancellationToken cancellationToken = default)
            where TResponse : Response, new()
        {
            var httpClient = CreateHttpClient();
            var item = await httpClient.GetFromJsonAsync<TResponse>($"{url}/{id}", cancellationToken);
            return item;
        }

        public virtual async Task<CommandResponse> PostAsJson<TRequest>(string url, TRequest request, CancellationToken cancellationToken = default)
            where TRequest : Request, new()
        {
            var httpClient = CreateHttpClient();
            var response = await httpClient.PostAsJsonAsync(url, request, cancellationToken);
            return response.IsSuccessStatusCode ? Success($"JSON post at {url} successful.", 0) : Error($"JSON post at {url} failed!");
        }
        public virtual async Task<CommandResponse> PutAsJson<TRequest>(string url, TRequest request, CancellationToken cancellationToken = default)
            where TRequest : Request, new()
        {
            var httpClient = CreateHttpClient();
            var response = await httpClient.PutAsJsonAsync(url, request, cancellationToken);
            return response.IsSuccessStatusCode ? Success($"JSON put at {url} successful.", request.Id) : Error($"JSON put at {url} failed!");
        }
        public virtual async Task<CommandResponse> Delete(string url, int id, CancellationToken cancellationToken = default)
        {
            var httpClient = CreateHttpClient();
            var response = await httpClient.DeleteAsync($"{url}/{id}", cancellationToken);
            return response.IsSuccessStatusCode ? Success($"Delete at {url} successful.", id) : Error($"Delete at {url} failed!");
        }
    }
}