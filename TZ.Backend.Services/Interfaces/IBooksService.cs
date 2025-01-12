using TZ.Backend.Models;

namespace TZ.Backend.Services.Interfaces
{
    public interface IBooksService
    {
        public Task<string> AddBook(Book book);
        public Task AddAuthor(Author author);
        public Task<IEnumerable<Book>> GetBooks(string? title, int? authorId, DateOnly? date, string? orderBy);
        public Task<int> GetBooksCount();
        public Task<bool> BuyBook(int id);
        public Task<bool> RestockBooks(int? id, int? count);
    }
}
