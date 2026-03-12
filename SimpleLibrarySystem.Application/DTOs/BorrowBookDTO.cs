using SimpleLibrarySystem.Domain.ValueObjects;

namespace SimpleLibrarySystem.Application.DTOs
{
    public class BorrowBookDTO
    {
        public Guid MemberID { get; set; }
        public Guid BookId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
    }
}
