using Microsoft.AspNetCore.Http;
using System.Text;
using System.Text.Json;

namespace CORE.APP.Services.Session.MVC
{
    // Abstract base class for session management.
    // Provides methods to store, retrieve, and remove complex objects in session using JSON serialization.
    public abstract class SessionServiceBase
    {
        protected IHttpContextAccessor HttpContextAccessor { get; }

        protected SessionServiceBase(IHttpContextAccessor httpContextAccessor)
        {
            HttpContextAccessor = httpContextAccessor;
        }

        public virtual T GetSession<T>(string key) where T : class
        {
            var value = HttpContextAccessor.HttpContext.Session.GetString(key);
            if (string.IsNullOrEmpty(value))
                return null;
            return JsonSerializer.Deserialize<T>(value); // Converts JSON string to object of type T
        }

        public virtual void SetSession<T>(string key, T instance) where T : class
        {
            if (instance is not null)
            {
                var value = JsonSerializer.Serialize(instance); 
                HttpContextAccessor.HttpContext.Session.SetString(key, value);
            }
        }

        public virtual void RemoveSession(string key)
        {
            HttpContextAccessor.HttpContext.Session.Remove(key);
        }
    }
}