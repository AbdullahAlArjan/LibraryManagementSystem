using System;

namespace Library.DataAccessLayer.Interfaces;

public interface IUserRepository
{
    public interface IUserRepository
{
    bool ValidateLogin(string username, string password);
    void AddUser(User user);
    void UpdateUser(User user);
    void DeleteUser(int userId);
    DataTable GetAllUsers();
    User GetUserById(int userId);
}

}
