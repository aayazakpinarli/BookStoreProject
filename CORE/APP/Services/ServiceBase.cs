using CORE.APP.Models;
using System.Globalization;

namespace CORE.APP.Services
{
    public abstract class ServiceBase
    {
        private CultureInfo _cultureInfo;

        protected CultureInfo CultureInfo
        {
            get
            {
                return _cultureInfo;
            }
            set
            {
                _cultureInfo = value;
                Thread.CurrentThread.CurrentCulture = _cultureInfo;
                Thread.CurrentThread.CurrentUICulture = _cultureInfo;
            }
        }

        protected ServiceBase()
        {
            CultureInfo = new CultureInfo("en-US"); 
        }

        protected CommandResponse Success(string message, int id) => new CommandResponse(true, message, id);

        protected CommandResponse Error(string message) => new CommandResponse(false, message);
    }
}