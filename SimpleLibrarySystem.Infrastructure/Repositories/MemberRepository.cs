using SimpleLibrarySystem.Domain.Interfaces;
using SimpleLibrarySystem.Domain.ValueObjects;
using SimpleLibrarySystem.Domain.Entities;
using System.Collections.Concurrent; 

namespace SimpleLibrarySystem.Infrastructure.Repositories
{
    public class MemberRepository : IMemberRepository
    {

        private static readonly List<Member> _mockMembers = new List<Member>()
        {
            Member.Create(Guid.NewGuid(), new PersonName("Alice"), Member.enMemberLevel.Premium),
            Member.Create(Guid.NewGuid(), new PersonName("Bob"), Member.enMemberLevel.Regular),
            Member.Create(Guid.NewGuid(), new PersonName("Charlie"), Member.enMemberLevel.Regular),
            Member.Create(Guid.NewGuid(), new PersonName("Diana"), Member.enMemberLevel.Premium),
            Member.Create(Guid.NewGuid(), new PersonName("Edward"), Member.enMemberLevel.Regular)
        };


        public Task AddAsync(Member member)
        {
            if (member == null) throw new ArgumentNullException(nameof(member));

            _mockMembers.Add(member);
            return Task.CompletedTask;

        }

        public Task DeleteAsync(Member member)
        {
            if (member == null) return Task.CompletedTask;

            _mockMembers.Remove(member);
            return Task.CompletedTask;
        }

        public Task<IEnumerable<Member>> GetAllAsync()
        {
            return Task.FromResult(_mockMembers.AsEnumerable());
        }

        public Task<Member?> GetAsync(Guid id)
        {
            var member = _mockMembers.FirstOrDefault(m => m.Id ==  id);

            return Task.FromResult(member);
        }

        public Task UpdateAsync(Member member)
        {
            if (member == null) throw new ArgumentNullException(nameof(member));

            var index = _mockMembers.FindIndex(m => m.Id == member.Id);

            if (index != -1)
            {
                _mockMembers[index] = member;
            }

            return Task.CompletedTask;
        }
    }
}