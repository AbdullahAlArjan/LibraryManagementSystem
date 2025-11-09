using System;
using System.Data;
using Library.Entities;
using Library.DataAccessLayer.Interfaces;

namespace Library.BusinessLayer
{
    public class UserManager
    {
        private readonly IUserRepository _userRepository;

        public UserManager(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public bool ValidateLogin(string username, string password)
        {
            return _userRepository.ValidateLogin(username, password);
        }

        public int AddUser(User user)
        {
            if (string.IsNullOrWhiteSpace(user.Username) || string.IsNullOrWhiteSpace(user.Password))
                throw new ArgumentException("Username and password are required.");

            return _userRepository.AddUser(user);
        }

        public bool UpdateUser(User user)
        {
            if (user == null || user.UserID <= 0)
                throw new ArgumentException("Invalid user data.");

            return _userRepository.UpdateUser(user);
        }

        public bool DeleteUser(int userId)
        {
            if (userId <= 0)
                throw new ArgumentException("Invalid user ID.");

            return _userRepository.DeleteUser(userId);
        }

        public DataTable GetAllUsers()
        {
            return _userRepository.GetAllUsers();
        }

        public User GetUserById(int userId)
        {
            if (userId <= 0)
                throw new ArgumentException("Invalid user ID.");

            return _userRepository.GetUserById(userId);
        }
    }
}
