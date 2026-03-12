using SimpleLibrarySystem.Domain.Common.Results;
using SimpleLibrarySystem.Domain.Entities;

namespace SimpleLibrarySystem.Domain.Interfaces
{
    public interface IMemberRepository
    {
        Task<Result> AddAsync(Member member);
        Task<Result> UpdateAsync(Member member);

        Task<Result> DeleteAsync(Member member);

        Task<IEnumerable<Member>> GetAllAsync();

        Task<ResultT<Member>> GetAsync(Guid id);
    }
}
