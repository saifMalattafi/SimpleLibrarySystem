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
            ResultT<Loan> loanResult = await _loanRepository.GetAsync(dto.LoanID);
            if (loanResult.IsFailure) return Result.Failure(loanResult.Error);

            var loan = loanResult.Value;

            if (loan.IsReturned) return Result.Failure("Book has been Returned, you can borrow it again.");

            var result = loan.Extend(dto.NewDueDate);
            if (result.IsFailure) return Result.Failure(result.Error);


            try
            {
                await _loanRepository.SaveAsync(loan);

                if (_notification.Notify("member.Email", "Extend processed successfully.").IsFailure)
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
