using SimpleLibrarySystem.Domain.Entities;
using SimpleLibrarySystem.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SimpleLibrarySystem.Infrastructure.Repositories
{
    public class LoanRepository : ILoanRepository
    {
        private static readonly List<Loan> _loans = new List<Loan>();

        public Task SaveAsync(Loan loan)
        {
            if (loan == null) throw new ArgumentNullException(nameof(loan));

            if (!_loans.Any(l => l.Id == loan.Id))
            {
                _loans.Add(loan);
            }
            return Task.CompletedTask;
        }

        public Task UpdateAsync(Loan loan)
        {
            var index = _loans.FindIndex(l => l.Id == loan.Id);
            if (index != -1)
            {
                _loans[index] = loan;
            }
            return Task.CompletedTask;
        }

        public Task DeleteAsync(Loan loan)
        {
            var existingLoan = _loans.FirstOrDefault(l => l.Id == loan.Id);
            if (existingLoan != null)
            {
                _loans.Remove(existingLoan);
            }
            return Task.CompletedTask;
        }

        public Task<IEnumerable<Loan>> GetAllAsync()
        {
            return Task.FromResult(_loans.AsEnumerable());
        }

        public Task<Loan?> GetAsync(Guid id)
        {
            var loan = _loans.FirstOrDefault(l => l.Id == id);
            return Task.FromResult(loan);
        }
    }
}
