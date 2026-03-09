using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SimpleLibrarySystem.Application.Applications.UseCases.LoanUseCases;
using SimpleLibrarySystem.Application.DTOs;

namespace SimpleLibrarySystem.API.Controllers.LoanController
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoanController : ControllerBase
    {
        private readonly ExtendLoanUseCase _extendLoanUseCase;

        public LoanController(ExtendLoanUseCase extendLoanUseCase)
        {
            _extendLoanUseCase = extendLoanUseCase;
        }

        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [HttpPost("extend", Name = nameof(ExtendLoan))]
        public async Task<IActionResult> ExtendLoan(ExtendLoanDTO dto)
        {
            if (dto == null)
                return BadRequest(new { Error = "Invalid Data." });

            var result = await _extendLoanUseCase.Execute(dto);

            if (result.IsFailure)
            {
                return BadRequest(new { Error = result.Error });
            }

            return Ok(new {Message = "Extending loan done Successfully"});
        }
    }
}
