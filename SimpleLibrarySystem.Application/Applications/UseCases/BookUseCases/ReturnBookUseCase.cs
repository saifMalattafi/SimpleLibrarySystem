using SimpleLibrarySystem.Domain.Common.Results;
using SimpleLibrarySystem.Application.Applications.Interfaces;
using SimpleLibrarySystem.Domain.Entities;
using SimpleLibrarySystem.Domain.Interfaces;
using SimpleLibrarySystem.Application.DTOs;


namespace SimpleLibrarySystem.Application.Applications.UseCases.BookUseCases
{
    public class ReturnBookUseCase
    {
        private readonly IBookRepository _bookRepository;
        private readonly ILoanRepository _loanRepository;
        private readonly IMemberRepository _memberRepository;
        private readonly INotificationService _notification;

        public ReturnBookUseCase(IBookRepository bookRepository, ILoanRepository loanRepository,
            IMemberRepository memberRepository, INotificationService notification)
        {
            _bookRepository = bookRepository;
            _loanRepository = loanRepository;
            _memberRepository = memberRepository;
            _notification = notification;
        }

        public async Task<Result> Execute(Guid loanId)
        {
            dynamic result = await _loanRepository.GetAsync(loanId);
            if (result.IsFailure) return Result.Failure(result.Error);

            Loan loan = result.Value;

            if (loan.IsReturned) return Result.Failure("Book has been returned before.");

            result = await _memberRepository.GetAsync(loan.MemberID);
            if (result.IsFailure) return Result.Failure(result.Error);

            Member member = result.Value;

            result = await _bookRepository.GetAsync(loan.BookID);
            if (result.IsFailure) return Result.Failure(result.Error);

            Book book = result.Value;

            try
            {

                member.ApplyFines(loan.CalculateFine(), "Fine Reason");

                loan.MarkAsReturned();
                book.MakeAvailable();
                member.DecrementLoans();

                // should be saved in a single transaction.
                await _loanRepository.UpdateAsync(loan);
                await _bookRepository.UpdateAsync(book);
                await _memberRepository.UpdateAsync(member);

                result = _notification.Notify("MemberEmail", "message that tell user the Returning has been done successfully.");
                if (result.IsFailure) return Result.Failure(result.Error);

                return Result.Success();
            }
            catch (ArgumentException ex) { return Result.Failure(ex.Message); }

        }
    }
}
