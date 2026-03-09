using SimpleLibrarySystem.Domain.Entities;

namespace SimpleLibrarySystem.Domain.Interfaces
{
    public interface IMemberRepository
    {
        Task AddAsync(Member member);
        Task UpdateAsync(Member member);

        Task DeleteAsync(Member member);

        Task<IEnumerable<Member>> GetAllAsync();

        Task<Member> GetAsync(Guid id);
    }
}
