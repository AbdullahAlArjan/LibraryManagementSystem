using System;
using System.Data;
using Library.BusinessLayer;

namespace LibraryManagementSystem.Presentation.Menus
{
    public static class MemberMenu
    {
        public static void Show()
        {
            bool exit = false;

            while (!exit)
            {
                Console.Clear();
                Console.WriteLine(" MEMBER PORTAL");
                Console.WriteLine("===================");
                Console.WriteLine("1. View All Available Books");
                Console.WriteLine("2. View My Loans");
                Console.WriteLine("3. Return a Book");
                Console.WriteLine("0. Back");
                Console.Write("\nSelect option: ");

                switch (Console.ReadLine())
                {
                    case "1":
                        ViewAvailableBooks();
                        break;
                    case "2":
                        ViewMyLoans();
                        break;
                    case "3":
                        ReturnBook();
                        break;
                    case "0":
                        exit = true;
                        break;
                    default:
                        Console.WriteLine("❌ Invalid choice. Press any key...");
                        Console.ReadKey();
                        break;
                }
            }
        }

        private static void ViewAvailableBooks()
        {
            Console.Clear();
            Console.WriteLine(" AVAILABLE BOOKS\n");

            BookManager bookManager = new BookManager();
            DataTable books = bookManager.GetAllBooks();

            if (books.Rows.Count == 0)
                Console.WriteLine("No books found.");
            else
            {
                foreach (DataRow row in books.Rows)
                {
                    Console.WriteLine($"[{row["BookID"]}] {row["Title"]} by {row["Author"]}");
                }
            }

            Console.WriteLine("\nPress any key to return...");
            Console.ReadKey();
        }

        private static void ViewMyLoans()
        {
            Console.Clear();
            Console.Write("Enter your Member ID: ");
            if (int.TryParse(Console.ReadLine(), out int memberId))
            {
                LoanManager loanManager = new LoanManager();
                DataTable loans = loanManager.GetLoansByMemberId(memberId);

                if (loans.Rows.Count == 0)
                    Console.WriteLine("No active loans found.");
                else
                {
                    foreach (DataRow row in loans.Rows)
                    {
                        Console.WriteLine($"LoanID: {row["LoanID"]} | BookID: {row["BookID"]} | Borrowed: {row["BorrowDate"]} | Due: {row["DueDate"]}");
                    }
                }
            }
            else
            {
                Console.WriteLine("Invalid ID.");
            }

            Console.WriteLine("\nPress any key to return...");
            Console.ReadKey();
        }

        private static void ReturnBook()
        {
            Console.Clear();
            Console.Write("Enter Loan ID to return: ");
            if (int.TryParse(Console.ReadLine(), out int loanId))
            {
                LoanManager loanManager = new LoanManager();
                bool success = loanManager.ReturnBook(loanId);

                if (success)
                    Console.WriteLine(" Book returned successfully!");
                else
                    Console.WriteLine(" Failed to return book.");
            }
            else
            {
                Console.WriteLine("Invalid input.");
            }

            Console.WriteLine("\nPress any key to return...");
            Console.ReadKey();
        }
    }
}
