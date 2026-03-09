using SimpleLibrarySystem.Domain.Entities;
using SimpleLibrarySystem.Domain.Interfaces;
using SimpleLibrarySystem.Domain.ValueObjects;
using static SimpleLibrarySystem.Domain.Entities.Book;

namespace SimpleLibrarySystem.Infrastructure.Repositories
{
    public class BookRepository : IBookRepository
    {
        private static readonly List<Book> _books = new List<Book>
        {
            new Book(Guid.NewGuid(), new ISBN("9780132350884"), "Clean Code", new PersonName("Robert"), enStatus.Available),
            new Book(Guid.NewGuid(), new ISBN("9780132350881"), "Design Patterns", new PersonName("Robert"), enStatus.Available),
            new Book(Guid.NewGuid(), new ISBN("9780132350882"), "C++", new PersonName("Robert"), enStatus.Available),
            new Book(Guid.NewGuid(), new ISBN("9780132350885"), "C# Language", new PersonName("Robert"), enStatus.Available),
            new Book(Guid.NewGuid(), new ISBN("9780201616224"), "The Pragmatic Programmer", new PersonName("Andrew"), enStatus.Borrowed),
            new Book(Guid.NewGuid(), new ISBN("9780134494166"), "Clean Architecture", new PersonName("Robert"), enStatus.InRepair)
        };

        public Task AddAsync(Book book)
        {
            if (book == null) throw new ArgumentNullException(nameof(book));
            _books.Add(book);
            return Task.CompletedTask;
        }

        public Task DeleteAsync(Book book)
        {
            var existingBook = _books.FirstOrDefault(b => b.ISBN?.Value == book.ISBN?.Value);
            if (existingBook != null)
            {
                _books.Remove(existingBook);
            }
            return Task.CompletedTask;
        }

        public Task<IEnumerable<Book>> GetAllAsync()
        {
            return Task.FromResult(_books.AsEnumerable());
        }

        public Task<Book?> GetAsync(Guid Id)
        {
            var book = _books.FirstOrDefault(b => b.Id == Id);
            return Task.FromResult(book);
        }

        public Task UpdateAsync(Book book)
        {
            var index = _books.FindIndex(b => b.ISBN?.Value == book.ISBN?.Value);
            if (index != -1)
            {
                _books[index] = book;
            }
            return Task.CompletedTask;
        }
    }
}
