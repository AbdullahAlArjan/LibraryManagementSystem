using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Library.DataAccessLayer.Interfaces;
using Library.DataAccessLayer;
namespace Library.BusinessLayer
{
    public class BookManager : IBookRepository
    {
        private readonly IBookRepository _bookRepository;

        public BookManager(IBookrepository bookRepository)
        {
            _bookRepository = bookRepository;
        }

        public void AddBook(Book book)
        {
            _bookRepository.AddBook(book);
        }
        public Book GetBookById(int id)
        {
            return _bookRepository.GetBookById(id);
        }
        public DataTable GetAllBooks()
        {
            return _bookRepository.GetAllBooks();
        }
        public void UpdateBook(Book book)
        {
            _bookRepository.UpdateBook(book);
        }

    }
}
