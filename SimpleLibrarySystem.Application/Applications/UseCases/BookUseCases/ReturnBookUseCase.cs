using SimpleLibrarySystem.Application.Applications.Common.Results;
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
            Loan loan = await _loanRepository.GetAsync(loanId);
            if (loan == null) return Result.Failure("Loan not Exists!");

            if (loan.IsReturned)
            {
                return Result.Failure("Book has been returned before.");
            }

            Book book = await _bookRepository.GetAsync(loan.BookID);
            Member member = await _memberRepository.GetAsync(loan.MemberID);

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

                _notification.Notify("MemberEmail", "message that tell user the Returning has been done successfully.");

                return Result.Success();
            }
            catch (ArgumentException ex) { return Result.Failure(ex.Message); }

        }
    }
}
