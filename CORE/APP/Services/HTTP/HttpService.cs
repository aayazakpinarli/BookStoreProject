using Microsoft.AspNetCore.Http;

namespace CORE.APP.Services.HTTP
{
    // Provides a concrete implementation of <see cref="HttpServiceBase"/> for HTTP operations
    public class HttpService : HttpServiceBase
    {
        public HttpService(IHttpContextAccessor httpContextAccessor, IHttpClientFactory httpClientFactory)
            : base(httpContextAccessor, httpClientFactory)
        {
        }
    }
}