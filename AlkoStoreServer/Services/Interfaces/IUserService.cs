namespace AlkoStoreServer.Services.Interfaces
{
    public interface IUserService
    {
        public Task<string> GetUserNameByEmail(string email); 
    }
}
