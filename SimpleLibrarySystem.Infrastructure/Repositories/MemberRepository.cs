using SimpleLibrarySystem.Domain.Interfaces;
using SimpleLibrarySystem.Domain.ValueObjects;
using SimpleLibrarySystem.Domain.Entities;
using System.Collections.Concurrent;
using SimpleLibrarySystem.Domain.Common.Results;
using System.Diagnostics.Metrics;

namespace SimpleLibrarySystem.Infrastructure.Repositories
{
    public class MemberRepository : IMemberRepository
    {

        private static readonly List<Member> _mockMembers = new List<Member>()
        {
            Member.Create(Guid.NewGuid(), PersonName.Create("Alice").Value, Member.enMemberLevel.Premium).Value,
            Member.Create(Guid.NewGuid(), PersonName.Create("Bob").Value, Member.enMemberLevel.Regular).Value,
            Member.Create(Guid.NewGuid(), PersonName.Create("Charlie").Value, Member.enMemberLevel.Regular).Value,
            Member.Create(Guid.NewGuid(), PersonName.Create("Diana").Value, Member.enMemberLevel.Premium).Value,
            Member.Create(Guid.NewGuid(), PersonName.Create("Edward").Value, Member.enMemberLevel.Regular).Value
        };


        public async Task<Result> AddAsync(Member member)
        {
            if (member == null) return Result.Failure("Member is null");

            _mockMembers.Add(member);
            return Result.Success();

        }

        public async Task<Result> DeleteAsync(Member member)
        {
            if (member == null) return Result.Failure("Member is null");

            _mockMembers.Remove(member);
            return Result.Success();
        }

        public async Task<IEnumerable<Member>> GetAllAsync()
        {
            return await Task.FromResult(_mockMembers.AsEnumerable());
        }

        public async Task<ResultT<Member?>> GetAsync(Guid id)
        {
            var member = _mockMembers.FirstOrDefault(m => m.Id ==  id);

            if (member == default) return ResultT<Member?>.Failure("member is not exists");

            return ResultT<Member?>.Success(member);
        }

        public async Task<Result> UpdateAsync(Member member)
        {
            if (member == null) return Result.Failure("Member is null");

            var index = _mockMembers.FindIndex(m => m.Id == member.Id);

            if (index != -1)
            {
                _mockMembers[index] = member;
            }

            return Result.Success();
        }
    }
}