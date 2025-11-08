using System;
using System.Data;
using System.Data.SqlClient;
using Library.Entities;

namespace Library.DataAccessLayer
{
    public class BookRepository : IBookRepository
    {
        public int AddBook(Book book)
        {
            int bookId = -1;

            using (SqlConnection connection = new SqlConnection(DataAccessSettings.ConnectionString))
            {
                string query = @"INSERT INTO Books
                                (Title, Author, ISBN, PublicationDate, Publisher, Category, Quantity, AvailableQuantity, CreatedDate)
                                OUTPUT INSERTED.BookId
                                VALUES (@Title, @Author, @ISBN, @PublicationDate, @Publisher, @Category, @Quantity, @AvailableQuantity, @CreatedDate);";

                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@Title", book.Title);
                command.Parameters.AddWithValue("@Author", book.Author);
                command.Parameters.AddWithValue("@ISBN", book.ISBN);
                command.Parameters.AddWithValue("@PublicationDate", book.PublicationDate);
                command.Parameters.AddWithValue("@Publisher", book.Publisher);
                command.Parameters.AddWithValue("@Category", book.Category);
                command.Parameters.AddWithValue("@Quantity", book.Quantity);
                command.Parameters.AddWithValue("@AvailableQuantity", book.AvailableQuantity);
                command.Parameters.AddWithValue("@CreatedDate", DateTime.Now);

                connection.Open();
                object result = command.ExecuteScalar();

                if (result != null && int.TryParse(result.ToString(), out int insertedId))
                    bookId = insertedId;
            }

            return bookId;
        }

        public Book GetBookById(int id)
        {
            Book book = null;

            using (SqlConnection connection = new SqlConnection(DataAccessSettings.ConnectionString))
            {
                string query = "SELECT * FROM Books WHERE BookId = @BookId";
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@BookId", id);

                connection.Open();
                SqlDataReader reader = command.ExecuteReader();

                if (reader.Read())
                {
                    book = new Book
                    {
                        BookId = (int)reader["BookId"],
                        Title = reader["Title"].ToString(),
                        Author = reader["Author"].ToString(),
                        ISBN = reader["ISBN"].ToString(),
                        PublicationDate = Convert.ToDateTime(reader["PublicationDate"]),
                        Publisher = reader["Publisher"].ToString(),
                        Category = reader["Category"].ToString(),
                        Quantity = Convert.ToInt32(reader["Quantity"]),
                        AvailableQuantity = Convert.ToInt32(reader["AvailableQuantity"]),
                        CreatedDate = Convert.ToDateTime(reader["CreatedDate"])
                    };
                }
                reader.Close();
            }

            return book;
        }

        public DataTable GetAllBooks()
        {
            DataTable dt = new DataTable();

            using (SqlConnection connection = new SqlConnection(DataAccessSettings.ConnectionString))
            {
                string query = "SELECT * FROM Books";
                SqlCommand command = new SqlCommand(query, connection);

                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                dt.Load(reader);
                reader.Close();
            }

            return dt;
        }

        public bool UpdateBook(Book book)
        {
            int rowsAffected = 0;

            using (SqlConnection connection = new SqlConnection(DataAccessSettings.ConnectionString))
            {
                string query = @"UPDATE Books 
                                SET Title = @Title, Author = @Author, ISBN = @ISBN,
                                    PublicationDate = @PublicationDate, Publisher = @Publisher,
                                    Category = @Category, Quantity = @Quantity, 
                                    AvailableQuantity = @AvailableQuantity
                                WHERE BookId = @BookId";

                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@BookId", book.BookId);
                command.Parameters.AddWithValue("@Title", book.Title);
                command.Parameters.AddWithValue("@Author", book.Author);
                command.Parameters.AddWithValue("@ISBN", book.ISBN);
                command.Parameters.AddWithValue("@PublicationDate", book.PublicationDate);
                command.Parameters.AddWithValue("@Publisher", book.Publisher);
                command.Parameters.AddWithValue("@Category", book.Category);
                command.Parameters.AddWithValue("@Quantity", book.Quantity);
                command.Parameters.AddWithValue("@AvailableQuantity", book.AvailableQuantity);

                connection.Open();
                rowsAffected = command.ExecuteNonQuery();
            }

            return rowsAffected > 0;
        }
    }
}
