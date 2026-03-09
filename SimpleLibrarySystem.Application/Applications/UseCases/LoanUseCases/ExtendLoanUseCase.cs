using SimpleLibrarySystem.Application.Applications.Common.Results;
using SimpleLibrarySystem.Application.DTOs;
using SimpleLibrarySystem.Domain.Entities;
using SimpleLibrarySystem.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleLibrarySystem.Application.Applications.UseCases.LoanUseCases
{
    public class ExtendLoanUseCase
    {
        private readonly ILoanRepository _loanRepository;

        public ExtendLoanUseCase(ILoanRepository loanRepository)
        {
            _loanRepository = loanRepository;
        }

        public async Task<Result> Execute(ExtendLoanDTO dto)
        {
            Loan loan = await _loanRepository.GetAsync(dto.LoanID);
            if (loan == null) return Result.Failure("Loan not Exists.");

            if (loan.IsReturned) return Result.Failure("Book has been Returned, you can borrow it again.");

            loan.Extend(dto.NewDueDate);

            await _loanRepository.SaveAsync(loan);
            return Result.Success();
        }
    }
}
