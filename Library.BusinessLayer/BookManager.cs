using System;
using System.Data;
using Library.Entities;
using Library.DataAccessLayer.Interfaces;
using Library.DataAccessLayer;

namespace Library.BusinessLayer
{
    public class BookManager
    {
        private readonly IBookRepository _bookRepository;

        // Constructor injection (for flexibility)
        public BookManager(IBookRepository bookRepository)
        {
            _bookRepository = bookRepository;
        }

        // Default constructor (for console use)
        public BookManager()
        {
            _bookRepository = new BookRepository();
        }

        public int AddBook(Book book)
        {
            if (string.IsNullOrWhiteSpace(book.Title))
                throw new ArgumentException("Book title is required.");
            if (string.IsNullOrWhiteSpace(book.Author))
                throw new ArgumentException("Book author is required.");

            return _bookRepository.AddBook(book);
        }

        public Book GetBookById(int id)
        {
            if (id <= 0)
                throw new ArgumentException("Invalid Book ID.");

            return _bookRepository.GetBookById(id);
        }

        public DataTable GetAllBooks()
        {
            return _bookRepository.GetAllBooks();
        }

        public bool UpdateBook(Book book)
        {
            if (book == null || book.BookID <= 0)
                throw new ArgumentException("Invalid book data.");

            return _bookRepository.UpdateBook(book);
        }
    }
}
