using Library.Entities;
using System;
using System.Data;

namespace Library.DataAccessLayer.Interfaces;

public interface ILoanRepository
{
    int AddLoan(Loan loan);
    bool ReturnBook(int loanId);
    DataTable GetActiveLoans();
    DataTable GetLoansByMemberId(int memberId);
}
