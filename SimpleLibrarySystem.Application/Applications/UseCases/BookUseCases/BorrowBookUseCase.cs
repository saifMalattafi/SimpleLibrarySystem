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
            ResultT<Member> memberResult = await _memberRepository.GetAsync(borrowBookDTO.MemberID);
            if (memberResult.IsFailure) return Result.Failure(memberResult.Error);
            var member = memberResult.Value;

            ResultT<Book> bookResult = await _bookRepository.GetAsync(borrowBookDTO.BookId);
            if (bookResult.IsFailure) return Result.Failure(bookResult.Error);
            var book = bookResult.Value;

            if (!book.IsAvailable) return Result.Failure("Book is currently unavailable.");
            if (!member.CanBorrow()) return Result.Failure("Member has exceeded limits or has outstanding fines.");

            var periodResult = Period.Create(borrowBookDTO.StartDate, borrowBookDTO.EndDate);
            if (periodResult.IsFailure) return Result.Failure(periodResult.Error);

            var loanResult = Loan.CreateLoan(Guid.NewGuid(), book.Id, member, periodResult.Value);
            if (loanResult.IsFailure) return Result.Failure(loanResult.Error);

            var loan = loanResult.Value;

            member.IncrementLoans();
            book.MarkAsBorrowed();

            try
            {
                await _loanRepository.SaveAsync(loan);
                await _bookRepository.UpdateAsync(book);
                await _memberRepository.UpdateAsync(member);

                if (_notification.Notify("member.Email", "Borrow processed successfully.").IsFailure)
                { /* log the error */}

                return Result.Success();
            }
            catch (Exception ex)
            {
                return Result.Failure("A persistence error occurred: " + ex.Message);
            }

}
    }
}
