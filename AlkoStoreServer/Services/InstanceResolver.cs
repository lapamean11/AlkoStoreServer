using AlkoStoreServer.Services.Interfaces;

namespace AlkoStoreServer.Services
{
    public class InstanceResolver : IInstanceResolver
    {
        private readonly IServiceProvider _serviceProvider;

        public InstanceResolver(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public T GetInstance<T>()
        {
            return _serviceProvider.GetRequiredService<T>();
        }

        public object GetInstance(Type type)
        {
            return _serviceProvider.GetRequiredService(type);
        }
    }

}
