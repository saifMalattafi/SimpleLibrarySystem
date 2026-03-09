using SimpleLibrarySystem.Domain.ValueObjects;

namespace SimpleLibrarySystem.Application.DTOs
{
    public class BorrowBookDTO
    {
        public Guid MemberID { get; set; }
        public Guid BookId { get; set; }
        public Period Period { get; set; }
    }
}
