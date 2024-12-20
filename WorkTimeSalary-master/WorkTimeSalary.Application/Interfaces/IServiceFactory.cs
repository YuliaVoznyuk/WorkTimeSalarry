namespace WorkTimeSalary.Application.Interfaces
{
    public interface IServiceFactory
    {
        T GetService<T>();
        void RegisterService(Type actionType);
        T GetServiceProvider<T>(Type serviceType);
    }
}