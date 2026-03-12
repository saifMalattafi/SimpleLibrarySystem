using SimpleLibrarySystem.Domain.Base;
using SimpleLibrarySystem.Domain.Common.Results;
using SimpleLibrarySystem.Domain.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleLibrarySystem.Domain.Entities
{
    public class Loan : AggregateRoot<Guid>
    {
        private readonly List<LoanExtension> _extensions;

        public enum enStatus { Active, Returned, Overdue }

        public Guid BookID { get; }
        public Guid MemberID { get; }
        public Period? BorrowedPeriod { get; private set; }
        public enStatus Status { get; private set; }

        public bool IsReturned => Status == enStatus.Returned;

        public IReadOnlyCollection<LoanExtension> Extensions => _extensions.AsReadOnly();

        private Loan(Guid id, Guid book, Guid member, Period? borrowedPeriod, enStatus status, List<LoanExtension> loanExtensions)
            : base(id)
        {
            BookID = book;
            MemberID = member;
            BorrowedPeriod = borrowedPeriod;
            Status = status;
            _extensions = loanExtensions;   
        }

        public static ResultT<Loan> CreateLoan(Guid id, Guid bookId, Member member, Period? borrowedPeriod)
        {
            if (member.MemberLevel == Member.enMemberLevel.Regular && borrowedPeriod?.DurationDays > 14)
                return ResultT<Loan>.Failure("BorrowPeriod cannot exceed 14 days for Regular members");

            if (member.MemberLevel == Member.enMemberLevel.Premium && borrowedPeriod?.DurationDays > 30)
                return ResultT<Loan>.Failure("BorrowPeriod cannot exceed 30 days for Premium members");

            return ResultT<Loan>.Success(new Loan(id, bookId, member.Id, borrowedPeriod, enStatus.Active, new List<LoanExtension> { }));
        }

        public void MarkAsReturned()
        {
            Status = enStatus.Returned;
        }

        public Money CalculateFine()
        {
            if (BorrowedPeriod.GetOverDueDays(DateTime.Now) < 0) return Money.Zero();

            return Money.Create((decimal)BorrowedPeriod.GetOverDueDays(DateTime.Now) * 1.00m).Value;
        }

        public Result Extend(DateTime newDueTime)
        {
            if (_extensions.Count >= 2)
                return Result.Failure("Loan cannot be extended more than twice.");

            var result = BorrowedPeriod?.Extend(newDueTime);
            if (result.IsFailure) return Result.Failure(result.Error);

            BorrowedPeriod = result.Value;
            _extensions.Add(new LoanExtension(Guid.NewGuid(), newDueTime));

            return Result.Success();
        }
    }
}
