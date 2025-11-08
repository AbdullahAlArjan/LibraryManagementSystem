using System;
using System.Data;

namespace Library.DataAccessLayer.Interfaces;

public interface ILoanRepository
{
    void AddLoan(Loan loan);
    void ReturnBook(int loadId);
    DataTable GetActiveLoans();
    DataTable GetLoansByMemberId(int memberId);
}
