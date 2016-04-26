namespace MiniCarSales.Repository
{
    public interface IUserRepository
    {
        bool ValidateUser(string username, string password);
    }
}
