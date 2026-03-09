using SimpleLibrarySystem.Application.Applications.Common.Results;
using SimpleLibrarySystem.Application.Applications.Interfaces;
using SimpleLibrarySystem.Application.DTOs;
using SimpleLibrarySystem.Domain.Entities;
using SimpleLibrarySystem.Domain.Interfaces;
using System.Linq.Expressions;

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
            Member member = await _memberRepository.GetAsync(borrowBookDTO.MemberID);
            if (member == null) return Result.Failure("Member Not Found!");

            Book book = await _bookRepository.GetAsync(borrowBookDTO.BookId);
            if (book == null) return Result.Failure("Book Not Found!");

            if (!book.IsAvailable) return Result.Failure("Book is already borrowed or in In Repair.");

            if (!member.CanBorrow()) return Result.Failure($"Member Exceeds the Allowed Fines/loan limits");

            try
            {
                Loan loan = Loan.CreateLoan(Guid.NewGuid(), book.Id, member, borrowBookDTO.Period);

                member.IncrementLoans();
                book.MarkAsBorrowed();

                // should be in a single transaction.
                await _loanRepository.SaveAsync(loan);
                await _bookRepository.UpdateAsync(book);
                await _memberRepository.UpdateAsync(member);

                _notification.Notify("MemberEmail", "message that tell user the Borrowing has been done successfully.");

                return Result.Success();
            }
            catch (ArgumentException ex)
            { return Result.Failure(ex.Message); }
        }
    }
}
