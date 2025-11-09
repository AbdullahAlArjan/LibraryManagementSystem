using System;
using LibraryManagementSystem.Presentation.Menus;

namespace LibraryManagementSystem.Presentation
{
    public class ApplicationRunner
    {
        public void Run()
        {
            bool exit = false;

            while (!exit)
            {
                Console.Clear();
                Console.WriteLine("=============================");
                Console.WriteLine("1. Administrator / Librarian Portal");
                Console.WriteLine("2. Member Portal");
                Console.WriteLine("0. Exit Application");
                Console.Write("\nPlease select an option: ");

                switch (Console.ReadLine())
                {
                    case "1":
                        UserMenu.Show();
                        break;
                    case "2":
                        MemberMenu.Show();
                        break;
                    case "0":
                        exit = true;
                        break;
                    default:
                        Console.WriteLine(" Invalid choice. Press any key...");
                        Console.ReadKey();
                        break;
                }
            }
        }
    }
}
