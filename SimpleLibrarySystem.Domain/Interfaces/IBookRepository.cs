using SimpleLibrarySystem.Domain.Common.Results;
using SimpleLibrarySystem.Domain.Entities;

namespace SimpleLibrarySystem.Domain.Interfaces
{
    public interface IBookRepository
    {
        Task<Result> AddAsync(Book book);
        Task UpdateAsync(Book book);

        Task DeleteAsync(Book book);

        Task<IEnumerable<Book>> GetAllAsync();

        Task<ResultT<Book>> GetAsync(Guid Id);
    }
}
