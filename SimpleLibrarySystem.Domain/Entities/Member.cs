using SimpleLibrarySystem.Domain.Base;
using SimpleLibrarySystem.Domain.Common.Results;
using SimpleLibrarySystem.Domain.ValueObjects;

namespace SimpleLibrarySystem.Domain.Entities
{
    public class Member : AggregateRoot<Guid>
    {
        private readonly List<Fine> _fines = new List<Fine>();

        public enum enMemberLevel { Regular, Premium }

        public PersonName Name { get; private set; }
        public enMemberLevel MemberLevel { get; private set; }
        public int TotalActiveLoans { get; private set; }
        public Money TotalFines { get; private set; }

        public IReadOnlyCollection<Fine> Fines => _fines.AsReadOnly();

        private Member(Guid id, PersonName name, enMemberLevel level, List<Fine> fines)
            : base(id)
        {
            Name = name;
            MemberLevel = level;
            TotalActiveLoans = 0;
            TotalFines = Money.Zero();
            _fines = fines; 
        }

        public static ResultT<Member> Create(Guid Id, PersonName name, enMemberLevel level)
        {
            return ResultT<Member>.Success(new Member(Id, name, level, new List<Fine> { }));
        }

        public bool CanBorrow()
        {
            if (TotalFines.Amount > 20.00m) return false;

            if (MemberLevel == enMemberLevel.Regular && TotalActiveLoans >= 3) return false;
            if (MemberLevel == enMemberLevel.Premium && TotalActiveLoans >= 10) return false;

            return true;
        }

        public void IncrementLoans() => TotalActiveLoans++;
        public void DecrementLoans() => TotalActiveLoans--;

        public void ApplyFines(Money amount, string reason)
        {
            var fine = new Fine(Guid.NewGuid(), amount, reason, DateTime.Now);
            _fines.Add(fine);

            TotalFines = TotalFines.Add(amount);
        }
    }
}