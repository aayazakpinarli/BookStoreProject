namespace CORE.APP.Services.Authentication.MVC
{
    // Defines methods for handling cookie-based authentication operations
    public interface ICookieAuthService
    {
        public Task SignIn(int userId, string userName, string[] userRoleNames, DateTime? expiration = default, bool isPersistent = true);

        public Task SignOut();
    }
}