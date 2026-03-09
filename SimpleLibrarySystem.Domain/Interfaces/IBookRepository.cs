using SimpleLibrarySystem.Domain.Entities;

namespace SimpleLibrarySystem.Domain.Interfaces
{
    public interface IBookRepository
    {
        Task AddAsync(Book book);
        Task UpdateAsync(Book book);

        Task DeleteAsync(Book book);

        Task<IEnumerable<Book>> GetAllAsync();

        Task<Book> GetAsync(Guid Id);
    }
}
