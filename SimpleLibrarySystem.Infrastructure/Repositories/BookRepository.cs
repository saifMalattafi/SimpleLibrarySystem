using SimpleLibrarySystem.Domain.Common.Results;
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
            new Book(Guid.NewGuid(), ISBN.Create("9780132350884").Value, "Clean Code", PersonName.Create("Robert").Value, enStatus.Available),
            new Book(Guid.NewGuid(), ISBN.Create("9780132350881").Value, "Design Patterns", PersonName.Create("Robert").Value, enStatus.Available),
            new Book(Guid.NewGuid(), ISBN.Create("9780132350882").Value, "C++", PersonName.Create("Robert").Value, enStatus.Available),
            new Book(Guid.NewGuid(), ISBN.Create("9780132350885").Value, "C# Language", PersonName.Create("Robert").Value, enStatus.Available),
            new Book(Guid.NewGuid(), ISBN.Create("9780201616224").Value, "The Pragmatic Programmer", PersonName.Create("Andrew").Value, enStatus.Borrowed),
            new Book(Guid.NewGuid(), ISBN.Create("9780134494166").Value, "Clean Architecture", PersonName.Create("Robert").Value, enStatus.InRepair)
        };

        public async Task<Result> AddAsync(Book book)
        {
            if (book == null) return Result.Failure("book is null");
            _books.Add(book);
            return Result.Success();
        }

        public Task DeleteAsync(Book book)
        {
            var existingBook = _books.FirstOrDefault(b => b.ISBN?.Value == book.ISBN?.Value);
            if (existingBook != default)
            {
                _books.Remove(existingBook);
            }
            return Task.CompletedTask;
        }

        public Task<IEnumerable<Book>> GetAllAsync()
        {
            return Task.FromResult(_books.AsEnumerable());
        }

        public async Task<ResultT<Book?>> GetAsync(Guid Id)
        {
            var book = _books.FirstOrDefault(b => b.Id == Id);

            if (book == default) return ResultT<Book?>.Failure("book is not Exists");

            return ResultT<Book?>.Success(book);
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
