using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SimpleLibrarySystem.Application.Applications.UseCases.BookUseCases;
using SimpleLibrarySystem.Application.DTOs;

namespace SimpleLibrarySystem.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookController : ControllerBase
    {
        private readonly BorrowBookUseCase _borrowUseCase;
        private readonly ReturnBookUseCase _returnUseCase;

        public BookController(BorrowBookUseCase borrowUseCase, ReturnBookUseCase returnUseCase)
        {
            _borrowUseCase = borrowUseCase;
            _returnUseCase = returnUseCase;
        }

        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [HttpPost("borrow", Name = nameof(BorrowBook))]
        public async Task<IActionResult> BorrowBook(BorrowBookDTO dto)
        {
            if (dto == null)
            {
                return BadRequest(new { Error = "Invalid Borrowing Data" });
            }

            var result = await _borrowUseCase.Execute(dto);

            if (result.IsFailure)
            {
                return BadRequest(new { Message = result.Error });
            }

            return Ok(new { Message = "Book borrowed Successfully." });
        }

        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [HttpPost("return", Name = nameof(ReturnBook))]
        public async Task<IActionResult> ReturnBook(Guid loanId)
        {
            var result = await _returnUseCase.Execute(loanId);

            return result.IsSuccess
            ? Ok(new { Message = "Book returned." })
            : BadRequest(new { result.Error });
        }
    }
}
