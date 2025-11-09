using Library.DataAccessLayer.Interfaces;
using Library.Entities;
using System;
using System.Data;

using Microsoft.Data.SqlClient;
namespace Library.DataAccessLayer;

public class LoanRepository : ILoanRepository
{
    public int AddLoan(Loan loan)
    {
        int loanId = -1;

        SqlConnection connection = new SqlConnection(DataAccessSettings.ConnectionString);

        string query = @"INSERT INTO Loans (MemberID,BookID,BorrowDate,DueDate,ReturnDate,FineAmount) 
                         VALUES (@MemberID,@BookID,@BorrowDate,@DueDate,@ReturnDate,@FineAmount);
                         SELECT SCOPE_IDENTITY();";
        SqlCommand command = new SqlCommand(query, connection);

        command.Parameters.AddWithValue("@MemberID", loan.MemberID);
        command.Parameters.AddWithValue("@BookID", loan.BookID);
        command.Parameters.AddWithValue("@BorrowDate", loan.BorrowDate);
        command.Parameters.AddWithValue("@DueDate", loan.DueDate);
        command.Parameters.AddWithValue("@ReturnDate", (object?)loan.ReturnDate ?? DBNull.Value);
        command.Parameters.AddWithValue("@FineAmount", loan.FineAmount);

        try

        {
            connection.Open();
            object result = command.ExecuteScalar();
            if (result != null && int.TryParse(result.ToString(), out int insertedID))
            {
                loanId = insertedID;
            }


        }
        catch (Exception ex)
        {

            throw new Exception("An error occurred while adding the loan.", ex);
        }
        finally
        {
            connection.Close();
        }

        return loanId;
    }

    public bool ReturnBook(int loanId)
    {
        int rowsAffected = 0;
        bool isReturned = false;
        SqlConnection connection = new SqlConnection(DataAccessSettings.ConnectionString);
        string query = @"UPDATE Loans 
                         SET ReturnDate = @ReturnDate 
                         WHERE LoanID = @LoanID AND ReturnDate IS NULL;";

        SqlCommand command = new SqlCommand(query, connection);

        command.Parameters.AddWithValue("@ReturnDate", DateTime.Now);
        command.Parameters.AddWithValue("@LoanID", loanId);

        try
        {
            connection.Open();
            rowsAffected = command.ExecuteNonQuery();
            isReturned = rowsAffected > 0;
        }
        catch (Exception ex)
        {
            throw new Exception("An error occurred while returning the book.", ex);
        }
        finally
        {
            connection.Close();
        }
        return isReturned;
    }

    public DataTable GetActiveLoans()
    {
        DataTable activeLoansTable = new DataTable();
        SqlConnection connection = new SqlConnection(DataAccessSettings.ConnectionString);
        string query = @"SELECT LoanID, MemberID, BookID, BorrowDate, DueDate 
                         FROM Loans 
                         WHERE ReturnDate IS NULL;";

        SqlCommand command = new SqlCommand(query, connection);

        try
        {
            connection.Open();
            SqlDataReader reader = command.ExecuteReader();
            if (reader.HasRows)
            {
                activeLoansTable.Load(reader);
            }
            reader.Close();
        }
        catch (Exception ex)
        {
            throw new Exception("An error occurred while retrieving active loans.", ex);
        }
        finally
        {
            connection.Close();
        }
        return activeLoansTable;
    }
    public DataTable GetLoansByMemberId(int memberId)
    {
        DataTable memberLoansTable = new DataTable();
        SqlConnection connection = new SqlConnection(DataAccessSettings.ConnectionString);
        string query = @"SELECT LoanID, BookID, BorrowDate, DueDate, ReturnDate, FineAmount 
                         FROM Loans 
                         WHERE MemberID = @MemberID;";

        SqlCommand command = new SqlCommand(query, connection);
        command.Parameters.AddWithValue("@MemberID", memberId);

        try
        {
            connection.Open();
            SqlDataReader reader = command.ExecuteReader();
            if (reader.HasRows)
            {
                memberLoansTable.Load(reader);
            }
            reader.Close();
        }
        catch (Exception ex)
        {
            throw new Exception("An error occurred while retrieving loans for the member.", ex);
        }
        finally
        {
            connection.Close();
        }
        return memberLoansTable;

    }

}
