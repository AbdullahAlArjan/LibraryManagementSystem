using Library.BusinessLayer;
using Library.DataAccessLayer;
using Library.DataAccessLayer.Interfaces;
using Library.Entities;
using System;
using Library.DataAccessLayer;
using Library.DataAccessLayer.Interfaces;


namespace LibraryManagementSystem.Presentation.Menus
{
    public static class UserMenu
    {
        private static bool _isLoggedIn = false;
        private static string _currentUsername = string.Empty;


        public static void Show()
        {
            if (!_isLoggedIn)
            {
                Login();
            }

            if (_isLoggedIn)
            {
                ShowAdminOptions();
            }
        }

        private static void Login()
        {
            Console.Clear();
            Console.WriteLine(" ADMIN LOGIN");
            Console.WriteLine("==============\n");

            Console.Write("Username: ");
            string username = Console.ReadLine();

            Console.Write("Password: ");
            string password = ReadPassword();

            // ✅ Create repository and inject it into manager
            IUserRepository userRepository = new UserRepository();
            UserManager userManager = new(userRepository);

            if (userManager.ValidateLogin(username, password))
            {
                _isLoggedIn = true;
                _currentUsername = username;
                Console.WriteLine("\n✅ Login successful!");
            }
            else
            {
                Console.WriteLine("\n❌ Invalid username or password!");
                Console.ReadKey();
            }
        }


        private static void ShowAdminOptions()
        {
            bool exit = false;

            while (!exit)
            {
                Console.Clear();
                Console.WriteLine($" Logged in as: {_currentUsername}");
                Console.WriteLine("===============================");
                Console.WriteLine("1. Manage Books");
                Console.WriteLine("2. Manage Members");
                Console.WriteLine("3. Manage Loans");
                Console.WriteLine("4. Logout");
                Console.Write("\nSelect option: ");

                switch (Console.ReadLine())
                {
                    case "1":
                        BookMenu.Show();
                        break;
                    case "2":
                        MemberMenu.Show();
                        break;
                    case "3":
                        LoanMenu.Show();
                        break;
                    case "4":
                        _isLoggedIn = false;
                        exit = true;
                        break;
                    default:
                        Console.WriteLine("❌ Invalid choice. Press any key...");
                        Console.ReadKey();
                        break;
                }
            }
        }

        private static string ReadPassword()
        {
            string password = "";
            ConsoleKey key;
            do
            {
                var keyInfo = Console.ReadKey(intercept: true);
                key = keyInfo.Key;

                if (key == ConsoleKey.Backspace && password.Length > 0)
                {
                    password = password.Substring(0, password.Length - 1);
                    Console.Write("\b \b");
                }
                else if (!char.IsControl(keyInfo.KeyChar))
                {
                    password += keyInfo.KeyChar;
                    Console.Write("*");
                }
            } while (key != ConsoleKey.Enter);

            Console.WriteLine();
            return password;
        }
        public static class BookMenu
        {
            public static void Show()
            {
                Console.WriteLine("Book Management Menu");
                // TODO: Add menu logic later
            }
        }
        public static class LoanMenu
        {
            public static void Show()
            {
                Console.WriteLine(" Loan Management Menu");
                // TODO: Add menu logic later
            }
        }
    }
}
