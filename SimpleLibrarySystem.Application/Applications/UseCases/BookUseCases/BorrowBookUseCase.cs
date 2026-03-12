using SimpleLibrarySystem.Domain.Common.Results;
using SimpleLibrarySystem.Application.Applications.Interfaces;
using SimpleLibrarySystem.Application.DTOs;
using SimpleLibrarySystem.Domain.Entities;
using SimpleLibrarySystem.Domain.Interfaces;
using System.Linq.Expressions;
using SimpleLibrarySystem.Domain.ValueObjects;

namespace SimpleLibrarySystem.Application.Applications.UseCases.BookUseCases
{
    public class BorrowBookUseCase
    {
        private readonly IBookRepository _bookRepository;
        private readonly IMemberRepository _memberRepository;
        private readonly ILoanRepository _loanRepository;
        private readonly INotificationService _notification;

        public BorrowBookUseCase(IBookRepository bookRepository, IMemberRepository memberRepository, ILoanRepository loanRepository, INotificationService notification)
        {
            _bookRepository = bookRepository;
            _memberRepository = memberRepository;
            _loanRepository = loanRepository;
            _notification = notification;
        }

        public async Task<Result> Execute(BorrowBookDTO borrowBookDTO)
        {
            dynamic result = await _memberRepository.GetAsync(borrowBookDTO.MemberID);
            if (result.IsFailure) return Result.Failure(result.Error);

            Member member = result.Value;

            result = await _bookRepository.GetAsync(borrowBookDTO.BookId);
            if (result.IsFailure) return Result.Failure(result.Error);

            Book book = result.Value;
            if (!book.IsAvailable) return Result.Failure("Book is already borrowed or in In Repair.");

            if (!member.CanBorrow()) return Result.Failure($"Member Exceeds the Allowed Fines/loan limits");

            try
            {
                result = Period.Create(borrowBookDTO.StartDate, borrowBookDTO.EndDate);
                if (result.IsFailure) return Result.Failure(result.Error);

                result = Loan.CreateLoan(Guid.NewGuid(), book.Id, member, result.Value);
                if (result.IsFailure) return Result.Failure(result.Error);

                Loan loan = result.Value;

                member.IncrementLoans();
                book.MarkAsBorrowed();

                // should be in a single transaction.
                await _loanRepository.SaveAsync(loan);
                await _bookRepository.UpdateAsync(book);
                await _memberRepository.UpdateAsync(member);

                result = _notification.Notify("MemberEmail", "message that tell user the Borrowing has been done successfully.");
                if (result.IsFailure) return Result.Failure(result.Error);

                return Result.Success();
            }
            catch (ArgumentException ex)
            { return Result.Failure(ex.Message); }
        }
    }
}
