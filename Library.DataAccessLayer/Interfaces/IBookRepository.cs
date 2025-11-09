using System;
using System.Collections.Concurrent;
using System.Data;
using Library.Entities;
using System.Data;

namespace Library.DataAccessLayer.Interfaces;

public interface IBookRepository
{
    int AddBook(Book book);
    Book GetBookById(int id);
    DataTable GetAllBooks();
    bool UpdateBook(Book book);
    bool DeleteBook(int id);
}
