using System;

namespace LibraryManagementSystem.Presentation;

public class ApplicationRunner
{
    private readonly UserManager _userManager = new UserManager();
    public void Run()
    {
        Console.WriteLine("Library Management System is running...");
        // Additional application logic can be added here.
    }
}
