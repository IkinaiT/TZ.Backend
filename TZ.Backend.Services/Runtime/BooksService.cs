using Microsoft.EntityFrameworkCore;
using TZ.Backend.DataBase;
using TZ.Backend.Models;
using TZ.Backend.Services.Interfaces;

namespace TZ.Backend.Services.Runtime
{
    public class BooksService : IBooksService
    {
        private readonly DataBaseContext _context;

        public BooksService(string postgreesConnectionString)
        {
            var optionsPostgresSQL = new DbContextOptionsBuilder<DataBaseContext>();

            optionsPostgresSQL.UseNpgsql(postgreesConnectionString);
            
            _context = new(optionsPostgresSQL.Options);
        }

        public BooksService(DbContextOptionsBuilder<DataBaseContext> postgresOptions)
        {
            _context = new(postgresOptions.Options);
        }

        public async Task<string> AddBook(Book book)
        {
            var author = _context.Authors.FirstOrDefault(_ => _.Id == book.AuthorId);

            if (author == null)
                return "Author not exist";

            var tempBook = new DataBase.Models.Book
            {
                Id = book.Id,
                Title = book.Title,
                AuthorId = book.AuthorId,
                Count = book.Count,
                Date = book.Date
            };

            _context.Books.Add(tempBook);

            await _context.SaveChangesAsync();

            return "OK";
        }

        public async Task AddAuthor(Author author)
        {
            var tempAuthor = new DataBase.Models.Author
            {
                Id = author.Id,
                FullName = author.FullName
            };

            _context.Authors.Add(tempAuthor);

            await _context.SaveChangesAsync();
        }

        public async Task<bool> BuyBook(int id)
        {
            var tempBook = _context.Books.FirstOrDefault(_ => _.Id == id);

            if(tempBook == null) 
                return false;

            tempBook.Count--;

            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<IEnumerable<Book>> GetBooks(string? title, int? authorId, DateOnly? date, string? orderBy)
        {
            var tempBooks = _context.Books
                .Where(_ => (title == null || _.Title.ToLower().Contains(title)) && (authorId == null || _.AuthorId == authorId) && (date == null || _.Date == date)).Include(_ => _.Author);

            var t = tempBooks.ToList();

            var result = orderBy switch
            {
                "title" => FromDBToLocal(tempBooks.OrderBy(x => x.Title)),
                "autor" => FromDBToLocal(tempBooks.OrderBy(x => x.AuthorId)),
                "date" => FromDBToLocal(tempBooks.OrderBy(x => x.Date)),
                "count" => FromDBToLocal(tempBooks.OrderBy(x => x.Count)),
                _ => FromDBToLocal(tempBooks.OrderBy(x => x.Id))
            };

            await Task.CompletedTask;

            return result;
        }

        public async Task<int> GetBooksCount()
        {
            var result = _context.Books.Count();

            await Task.CompletedTask;

            return result;
        }

        public async Task<bool> RestockBooks(int? id, int? count)
        {
            var book = new DataBase.Models.Book();
            Random rand = new();

            if (id == null)
            {
                book = _context.Books.ToList()[rand.Next(await GetBooksCount())];
            }
            else
            {
                book = _context.Books.FirstOrDefault(_ => _.Id == id);
            }

            if(book == null) 
                return false;

            book.Count += count == null ? rand.Next(1, 100) : count ?? 0;

            return true;
        }


        private IEnumerable<Book> FromDBToLocal(IEnumerable<DataBase.Models.Book> books)
        {
            return books.Select(_ => new Book
            {
                AuthorId = _.AuthorId,
                Date = _.Date,
                Count = _.Count,
                Id = _.Id,
                Title = _.Title, 
                AuthorName = _.Author?.FullName ?? ""
            });
        }
    }
}
