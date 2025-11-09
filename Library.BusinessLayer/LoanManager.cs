using System;
using System.Data;
using Library.Entities;
using Library.DataAccessLayer.Interfaces;
using Library.DataAccessLayer;

namespace Library.BusinessLayer
{
    public class LoanManager
    {
        private readonly ILoanRepository _loanRepository;
        private readonly IBookRepository _bookRepository;

        // Constructor injection (for flexibility)
        public LoanManager(ILoanRepository loanRepository, IBookRepository bookRepository)
        {
            _loanRepository = loanRepository;
            _bookRepository = bookRepository;
        }

        // Default constructor (for console use)
        public LoanManager()
        {
            _loanRepository = new LoanRepository();
            _bookRepository = new BookRepository();
        }

        public int AddLoan(Loan loan)
        {
            if (loan == null || loan.MemberID <= 0 || loan.BookID <= 0)
                throw new ArgumentException("Invalid loan data.");

            return _loanRepository.AddLoan(loan);
        }

        public bool ReturnBook(int loanId)
        {
            if (loanId <= 0)
                throw new ArgumentException("Invalid loan ID.");

            return _loanRepository.ReturnBook(loanId);
        }

        public DataTable GetActiveLoans()
        {
            return _loanRepository.GetActiveLoans();
        }

        public DataTable GetLoansByMemberId(int memberId)
        {
            if (memberId <= 0)
                throw new ArgumentException("Invalid member ID.");

            return _loanRepository.GetLoansByMemberId(memberId);
        }
    }
}
