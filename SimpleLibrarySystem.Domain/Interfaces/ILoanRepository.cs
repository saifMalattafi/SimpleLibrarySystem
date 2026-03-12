using SimpleLibrarySystem.Domain.Common.Results;
using SimpleLibrarySystem.Domain.Entities;

namespace SimpleLibrarySystem.Domain.Interfaces
{
    public interface ILoanRepository
    {
        Task<Result> SaveAsync(Loan loan);
        Task UpdateAsync(Loan loan);

        Task DeleteAsync(Loan loan);

        Task<IEnumerable<Loan>> GetAllAsync();

        Task<ResultT<Loan>> GetAsync(Guid id);
    }
}
