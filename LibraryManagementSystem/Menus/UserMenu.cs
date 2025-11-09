using Library.BusinessLayer;
using Library.DataAccessLayer;
using Library.DataAccessLayer.Interfaces;
using Library.Entities;
using System;
using System.Data;


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
                Console.WriteLine("2. Manage Loans");
                Console.WriteLine("3. Logout");
                Console.Write("\nSelect option: ");

                switch (Console.ReadLine())
                {
                    case "1":
                        BookMenu.Show();
                        break;
                    case "2":
                        LoanMenu.Show();
                        break;
                    case "3":
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
                bool exit = false;
                BookManager bookManager = new BookManager();

                while (!exit)
                {
                    Console.Clear();
                    Console.WriteLine(" BOOK MANAGEMENT");
                    Console.WriteLine("===================");
                    Console.WriteLine("1. View All Books");
                    Console.WriteLine("2. Add New Book");
                    Console.WriteLine("3. Update Book");
                    Console.WriteLine("4. Delete Book");
                    Console.WriteLine("0. Back");
                    Console.Write("\nSelect option: ");

                    switch (Console.ReadLine())
                    {
                        case "1":
                            ViewAllBooks(bookManager);
                            break;
                        case "2":
                            AddBook(bookManager);
                            break;
                        case "3":
                            UpdateBook(bookManager);
                            break;
                        case "4":
                            DeleteBook(bookManager);
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

            private static void ViewAllBooks(BookManager bookManager)
            {
                Console.Clear();
                Console.WriteLine(" ALL BOOKS\n");
                Console.WriteLine(new string('-', 100));

                DataTable books = bookManager.GetAllBooks();
                if (books.Rows.Count == 0)
                {
                    Console.WriteLine("No books found.");
                }
                else
                {
                    Console.WriteLine($"{"ID",-5} {"Title",-30} {"Author",-20} {"Available",-12} {"Total",-8}");
                    Console.WriteLine(new string('-', 100));
                    foreach (DataRow row in books.Rows)
                    {
                        string title = row["Title"]?.ToString() ?? "";
                        string author = row["Author"]?.ToString() ?? "";
                        if (title.Length > 28) title = title.Substring(0, 28);
                        if (author.Length > 18) author = author.Substring(0, 18);
                        Console.WriteLine($"{row["BookID"],-5} {title,-30} {author,-20} {row["AvailableQuantity"],-12} {row["Quantity"],-8}");
                    }
                }

                Console.WriteLine("\nPress any key to return...");
                Console.ReadKey();
            }

            private static void AddBook(BookManager bookManager)
            {
                Console.Clear();
                Console.WriteLine(" ADD NEW BOOK\n");

                try
                {
                    Book book = new Book();

                    Console.Write("Title: ");
                    book.Title = Console.ReadLine();

                    Console.Write("Author: ");
                    book.Author = Console.ReadLine();

                    Console.Write("ISBN (optional): ");
                    string isbn = Console.ReadLine();
                    book.ISBN = string.IsNullOrWhiteSpace(isbn) ? null : isbn;

                    Console.Write("Publication Date (YYYY-MM-DD, optional): ");
                    string pubDate = Console.ReadLine();
                    if (!string.IsNullOrWhiteSpace(pubDate))
                    {
                        if (DateTime.TryParse(pubDate, out DateTime parsedDate))
                        {
                            book.PublicationDate = parsedDate;
                        }
                        else
                        {
                            Console.WriteLine("⚠️ Invalid date format. Skipping publication date.");
                            book.PublicationDate = null;
                        }
                    }
                    else
                    {
                        book.PublicationDate = null;
                    }

                    Console.Write("Publisher (optional): ");
                    book.Publisher = Console.ReadLine();

                    Console.Write("Category (optional): ");
                    book.Category = Console.ReadLine();

                    Console.Write("Quantity: ");
                    book.Quantity = int.Parse(Console.ReadLine());

                    Console.Write("Available Quantity: ");
                    book.AvailableQuantity = int.Parse(Console.ReadLine());

                    book.CreatedDate = DateTime.Now;

                    int bookId = bookManager.AddBook(book);
                    Console.WriteLine($"\n✅ Book added successfully! Book ID: {bookId}");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"\n❌ Error: {ex.Message}");
                }

                Console.WriteLine("\nPress any key to return...");
                Console.ReadKey();
            }

            private static void UpdateBook(BookManager bookManager)
            {
                Console.Clear();
                Console.WriteLine(" UPDATE BOOK\n");

                try
                {
                    Console.Write("Enter Book ID to update: ");
                    int bookId = int.Parse(Console.ReadLine());

                    Book book = bookManager.GetBookById(bookId);
                    if (book == null)
                    {
                        Console.WriteLine("❌ Book not found.");
                    }
                    else
                    {
                        Console.WriteLine($"\nCurrent Book Information:");
                        Console.WriteLine($"Title: {book.Title}");
                        Console.WriteLine($"Author: {book.Author}");
                        Console.WriteLine($"ISBN: {book.ISBN}");
                        Console.WriteLine($"\nEnter new values (press Enter to keep current value):\n");

                        Console.Write($"Title [{book.Title}]: ");
                        string title = Console.ReadLine();
                        book.Title = string.IsNullOrWhiteSpace(title) ? book.Title : title;

                        Console.Write($"Author [{book.Author}]: ");
                        string author = Console.ReadLine();
                        book.Author = string.IsNullOrWhiteSpace(author) ? book.Author : author;

                        Console.Write($"ISBN [{(book.ISBN ?? "none")}]: ");
                        string isbn = Console.ReadLine();
                        book.ISBN = string.IsNullOrWhiteSpace(isbn) ? book.ISBN : (isbn == "none" ? null : isbn);

                        Console.Write($"Quantity [{book.Quantity}]: ");
                        string quantity = Console.ReadLine();
                        if (!string.IsNullOrWhiteSpace(quantity))
                            book.Quantity = int.Parse(quantity);

                        Console.Write($"Available Quantity [{book.AvailableQuantity}]: ");
                        string availableQty = Console.ReadLine();
                        if (!string.IsNullOrWhiteSpace(availableQty))
                            book.AvailableQuantity = int.Parse(availableQty);

                        bool success = bookManager.UpdateBook(book);
                        if (success)
                            Console.WriteLine("\n✅ Book updated successfully!");
                        else
                            Console.WriteLine("\n❌ Failed to update book.");
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"\n❌ Error: {ex.Message}");
                }

                Console.WriteLine("\nPress any key to return...");
                Console.ReadKey();
            }

            private static void DeleteBook(BookManager bookManager)
            {
                Console.Clear();
                Console.WriteLine(" DELETE BOOK\n");

                try
                {
                    Console.Write("Enter Book ID to delete: ");
                    int bookId = int.Parse(Console.ReadLine());

                    Book book = bookManager.GetBookById(bookId);
                    if (book == null)
                    {
                        Console.WriteLine("❌ Book not found.");
                    }
                    else
                    {
                        Console.WriteLine($"\nBook to delete: {book.Title} by {book.Author}");
                        Console.Write("Are you sure? (y/n): ");
                        string confirm = Console.ReadLine();

                        if (confirm.ToLower() == "y")
                        {
                            IBookRepository bookRepo = new BookRepository();
                            bool success = bookRepo.DeleteBook(bookId);
                            if (success)
                                Console.WriteLine("\n✅ Book deleted successfully!");
                            else
                                Console.WriteLine("\n❌ Failed to delete book.");
                        }
                        else
                        {
                            Console.WriteLine("\nDelete cancelled.");
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"\n❌ Error: {ex.Message}");
                }

                Console.WriteLine("\nPress any key to return...");
                Console.ReadKey();
            }
        }

        public static class LoanMenu
        {
            public static void Show()
            {
                bool exit = false;
                LoanManager loanManager = new LoanManager();
                BookManager bookManager = new BookManager();
                MemberManager memberManager = new MemberManager(new MemberRepository());

                while (!exit)
                {
                    Console.Clear();
                    Console.WriteLine(" LOAN MANAGEMENT");
                    Console.WriteLine("===================");
                    Console.WriteLine("1. View All Active Loans");
                    Console.WriteLine("2. Create New Loan");
                    Console.WriteLine("3. Return Book");
                    Console.WriteLine("0. Back");
                    Console.Write("\nSelect option: ");

                    switch (Console.ReadLine())
                    {
                        case "1":
                            ViewAllActiveLoans(loanManager);
                            break;
                        case "2":
                            CreateLoan(loanManager, bookManager, memberManager);
                            break;
                        case "3":
                            ReturnBook(loanManager);
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

            private static void ViewAllActiveLoans(LoanManager loanManager)
            {
                Console.Clear();
                Console.WriteLine(" ACTIVE LOANS\n");
                Console.WriteLine(new string('-', 80));

                DataTable loans = loanManager.GetActiveLoans();
                if (loans.Rows.Count == 0)
                {
                    Console.WriteLine("No active loans found.");
                }
                else
                {
                    Console.WriteLine($"{"Loan ID",-10} {"Member ID",-12} {"Book ID",-10} {"Borrow Date",-15} {"Due Date",-15}");
                    Console.WriteLine(new string('-', 80));
                    foreach (DataRow row in loans.Rows)
                    {
                        DateTime borrowDate = Convert.ToDateTime(row["BorrowDate"]);
                        DateTime dueDate = Convert.ToDateTime(row["DueDate"]);
                        Console.WriteLine($"{row["LoanID"],-10} {row["MemberID"],-12} {row["BookID"],-10} {borrowDate.ToString("yyyy-MM-dd"),-15} {dueDate.ToString("yyyy-MM-dd"),-15}");
                    }
                }

                Console.WriteLine("\nPress any key to return...");
                Console.ReadKey();
            }

            private static void CreateLoan(LoanManager loanManager, BookManager bookManager, MemberManager memberManager)
            {
                Console.Clear();
                Console.WriteLine(" CREATE NEW LOAN\n");

                try
                {
                    Console.Write("Member ID: ");
                    int memberId = int.Parse(Console.ReadLine());

                    Member member = memberManager.GetMemberById(memberId);
                    if (member == null)
                    {
                        Console.WriteLine("❌ Member not found.");
                        Console.WriteLine("\nPress any key to return...");
                        Console.ReadKey();
                        return;
                    }

                    Console.Write("Book ID: ");
                    int bookId = int.Parse(Console.ReadLine());

                    Book book = bookManager.GetBookById(bookId);
                    if (book == null)
                    {
                        Console.WriteLine("❌ Book not found.");
                        Console.WriteLine("\nPress any key to return...");
                        Console.ReadKey();
                        return;
                    }

                    if (book.AvailableQuantity <= 0)
                    {
                        Console.WriteLine("❌ Book is not available.");
                        Console.WriteLine("\nPress any key to return...");
                        Console.ReadKey();
                        return;
                    }

                    Console.Write("Number of days to loan (default 14): ");
                    string daysInput = Console.ReadLine();
                    int days = string.IsNullOrWhiteSpace(daysInput) ? 14 : int.Parse(daysInput);

                    Loan loan = new Loan
                    {
                        MemberID = memberId,
                        BookID = bookId,
                        BorrowDate = DateTime.Now,
                        DueDate = DateTime.Now.AddDays(days),
                        ReturnDate = null,
                        FineAmount = 0
                    };

                    int loanId = loanManager.AddLoan(loan);
                    Console.WriteLine($"\n✅ Loan created successfully! Loan ID: {loanId}");

                    book.AvailableQuantity--;
                    bookManager.UpdateBook(book);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"\n❌ Error: {ex.Message}");
                }

                Console.WriteLine("\nPress any key to return...");
                Console.ReadKey();
            }

            private static void ReturnBook(LoanManager loanManager)
            {
                Console.Clear();
                Console.WriteLine(" RETURN BOOK\n");

                try
                {
                    Console.Write("Enter Loan ID to return: ");
                    int loanId = int.Parse(Console.ReadLine());

                    DataTable activeLoans = loanManager.GetActiveLoans();
                    DataRow loanRow = null;
                    foreach (DataRow row in activeLoans.Rows)
                    {
                        if (Convert.ToInt32(row["LoanID"]) == loanId)
                        {
                            loanRow = row;
                            break;
                        }
                    }

                    if (loanRow == null)
                    {
                        Console.WriteLine("\n❌ Loan not found or already returned.");
                    }
                    else
                    {
                        int bookId = Convert.ToInt32(loanRow["BookID"]);
                        bool success = loanManager.ReturnBook(loanId);
                        if (success)
                        {
                            BookManager bookManager = new BookManager();
                            Book book = bookManager.GetBookById(bookId);
                            if (book != null)
                            {
                                book.AvailableQuantity++;
                                bookManager.UpdateBook(book);
                            }
                            Console.WriteLine("\n✅ Book returned successfully!");
                        }
                        else
                        {
                            Console.WriteLine("\n❌ Failed to return book.");
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"\n❌ Error: {ex.Message}");
                }

                Console.WriteLine("\nPress any key to return...");
                Console.ReadKey();
            }
        }
    }
}
