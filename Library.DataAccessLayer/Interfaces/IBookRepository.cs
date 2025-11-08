using System;
using System.Collections.Concurrent;
using System.Data;

namespace Library.DataAccessLayer.Interfaces;

public interface IBookRepository
{
    void AddBook(Book book);
    Book GetBookById(int id);
    DataTable GetAllBooks();
    void UpdateBook(Book book);
    void DeleteBook(int id);
}
