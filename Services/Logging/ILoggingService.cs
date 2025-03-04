using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Dream_Bridge.Services.Logging
{
    public interface ILoggingService
    {
        void LogInformation(string message);
        void LogError(string message);
    }
}
