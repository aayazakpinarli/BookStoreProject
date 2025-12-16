using Microsoft.AspNetCore.Http;

namespace CORE.APP.Services.Session.MVC
{
    /// Concrete implementation of SessionServiceBase for session management
    public class SessionService : SessionServiceBase
    {
        public SessionService(IHttpContextAccessor httpContextAccessor) : base(httpContextAccessor)
        {
        }
    }
}