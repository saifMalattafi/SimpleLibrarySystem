using SimpleLibrarySystem.Domain.Common.Results;
using SimpleLibrarySystem.Application.DTOs;
using SimpleLibrarySystem.Domain.Entities;
using SimpleLibrarySystem.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SimpleLibrarySystem.Application.Applications.Interfaces;

namespace SimpleLibrarySystem.Application.Applications.UseCases.LoanUseCases
{
    public class ExtendLoanUseCase
    {
        private readonly ILoanRepository _loanRepository;
        private readonly INotificationService _notification;

        public ExtendLoanUseCase(ILoanRepository loanRepository, INotificationService notification)
        {
            _loanRepository = loanRepository;
            _notification = notification;
        }

        public async Task<Result> Execute(ExtendLoanDTO dto)
        {
            dynamic result = await _loanRepository.GetAsync(dto.LoanID);
            if (result.IsFailure) return Result.Failure(result.Error);

            Loan loan = result.Value;

            if (loan.IsReturned) return Result.Failure("Book has been Returned, you can borrow it again.");

            result = loan.Extend(dto.NewDueDate);
            if (result.IsFailure) return Result.Failure(result.Error);

            result = _notification.Notify("MemberEmail", "message that tell user the Extending Loan has been done successfully.");
            if (result.IsFailure) return Result.Failure(result.Error);

            await _loanRepository.SaveAsync(loan);
            return Result.Success();
        }
    }
}
