using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Library.DataAccessLayer.Interfaces;
using Library.DataAccessLayer;


namespace Library.BusinessLayer
{
    public class LoanManager : ILoanRepository
    {
        private readonly ILoanRepository _loanRepository;
        private readonly IBookRepository _bookRepository;

        public LoanManager(ILoanRepository loanRepository, IBookRepository bookRepository)
        {
            _loanRepository = loanRepository;
            _bookRepository = bookRepository;
        }

        public void AddLoan(Loan loan)
        {
            _loanRepository.AddLoan(loan);
        }
        public void ReturnBook(int loanId)
        {
            _loanRepository.ReturnBook(loanId);
        }
        public DataTable GetActiveLoans()
        {
            return _loanRepository.GetActiveLoans();
        }
        public DataTable GetLoansByMemberId(int memberId)
        {
            return _loanRepository.GetLoansByMemberId(memberId);
        }

    }
}
