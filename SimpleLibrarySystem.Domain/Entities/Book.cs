using SimpleLibrarySystem.Domain.Base;
using SimpleLibrarySystem.Domain.ValueObjects;

namespace SimpleLibrarySystem.Domain.Entities
{
    public class Book : Entity<Guid>
    {

        public enum enStatus { Available, Borrowed, InRepair }

        public ISBN? ISBN { get; private set; }
        public string? Title { get; private set; }
        public PersonName? Author { get; private set; }
        public enStatus? Status { get; private set; }

        public bool IsAvailable { get { return Status == enStatus.Available; } }

        public Book(Guid id, ISBN? iSBN, string? title, PersonName? author, enStatus? status = enStatus.Available)
            : base(id)
        {
            if (title == null || author == null) throw new ArgumentNullException("title/author can not be null...");

            ISBN = iSBN;
            Title = title;
            Author = author;
            Status = status;
        }

        public void MarkAsBorrowed()
        {
            Status = enStatus.Borrowed;
        }

        public void MakeAvailable()
        {
            Status = enStatus.Available;
        }

        public void Repair()
        {
            Status = enStatus.InRepair;
        }
    }
}
