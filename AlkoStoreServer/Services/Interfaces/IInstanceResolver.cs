namespace AlkoStoreServer.Services.Interfaces
{
    public interface IInstanceResolver
    {
        T GetInstance<T>();

        object GetInstance(Type type);
    }
}
