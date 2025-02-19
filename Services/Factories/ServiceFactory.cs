using Dream_Bridge.Services.Adapters;
using Dream_Bridge.Services.Logging; // Chỉ cần dòng này
using Dream_Bridge.Services.Repositories;
using Dream_Bridge.Services.Logging;
using Microsoft.Extensions.DependencyInjection;
using  Dream_Bridge.Services.Adapters;
using  Dream_Bridge.Services.Repositories;



namespace Dream_Bridge.Services.Factories
{
    public interface IServiceFactory
    {
        IApiAdapter CreateApiAdapter();
        ILoggingService CreateLoggingService();
        IRepository<T> CreateRepository<T>() where T : class;
    }

    public class ServiceFactory : IServiceFactory
    {
        private readonly IServiceProvider _serviceProvider;

        public ServiceFactory(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public IApiAdapter CreateApiAdapter()
        {
            return _serviceProvider.GetRequiredService<IApiAdapter>();
        }

        public ILoggingService CreateLoggingService()
        {
            return _serviceProvider.GetRequiredService<ILoggingService>();
        }

        public IRepository<T> CreateRepository<T>() where T : class
        {
            return _serviceProvider.GetRequiredService<IRepository<T>>();
        }
    }
}
