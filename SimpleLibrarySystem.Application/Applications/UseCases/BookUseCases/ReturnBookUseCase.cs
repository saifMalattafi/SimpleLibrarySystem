using SimpleLibrarySystem.Domain.Common.Results;
using SimpleLibrarySystem.Application.Applications.Interfaces;
using SimpleLibrarySystem.Domain.Entities;
using SimpleLibrarySystem.Domain.Interfaces;

namespace SimpleLibrarySystem.Application.Applications.UseCases.BookUseCases
{
    public class ReturnBookUseCase
    {
        private readonly IBookRepository _bookRepository;
        private readonly ILoanRepository _loanRepository;
        private readonly IMemberRepository _memberRepository;
        private readonly INotificationService _notification;

        public ReturnBookUseCase(
            IBookRepository bookRepository,
            ILoanRepository loanRepository,
            IMemberRepository memberRepository,
            INotificationService notification)
        {
            _bookRepository = bookRepository;
            _loanRepository = loanRepository;
            _memberRepository = memberRepository;
            _notification = notification;
        }

        public async Task<Result> Execute(Guid loanId)
        {
            ResultT<Loan> loanResult = await _loanRepository.GetAsync(loanId);
            if (loanResult.IsFailure) return Result.Failure(loanResult.Error);
            var loan = loanResult.Value;

            if (loan.IsReturned) return Result.Failure("Book has already been returned.");

            ResultT<Member> memberResult = await _memberRepository.GetAsync(loan.MemberID);
            if (memberResult.IsFailure) return Result.Failure(memberResult.Error);
            var member = memberResult.Value;

            ResultT<Book> bookResult = await _bookRepository.GetAsync(loan.BookID);
            if (bookResult.IsFailure) return Result.Failure(bookResult.Error);
            var book = bookResult.Value;

            member.ApplyFines(loan.CalculateFine(), $"Overdue: {loan.Id}");

            loan.MarkAsReturned();
            book.MakeAvailable();
            member.DecrementLoans();

            try
            {
                await _loanRepository.UpdateAsync(loan);
                await _bookRepository.UpdateAsync(book);
                await _memberRepository.UpdateAsync(member);

                _notification.Notify("member.Email", "Return processed successfully.");

                return Result.Success();
            }
            catch (Exception ex)
            {
                return Result.Failure("A persistence error occurred. Data may be inconsistent.");
            }
        }
    }
}