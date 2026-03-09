using SimpleLibrarySystem.Domain.Entities;

namespace SimpleLibrarySystem.Domain.Interfaces
{
    public interface ILoanRepository
    {
        Task SaveAsync(Loan loan);
        Task UpdateAsync(Loan loan);

        Task DeleteAsync(Loan loan);

        Task<IEnumerable<Loan>> GetAllAsync();

        Task<Loan> GetAsync(Guid id);
    }
}
