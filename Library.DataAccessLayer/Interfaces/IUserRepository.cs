using Library.Entities;
using System.Data;

namespace Library.DataAccessLayer.Interfaces
{
    public interface IUserRepository
    {
        bool ValidateLogin(string username, string password);
        int AddUser(User user);
        bool UpdateUser(User user);
        bool DeleteUser(int userId);
        DataTable GetAllUsers();
        User GetUserById(int userId);
    }
}