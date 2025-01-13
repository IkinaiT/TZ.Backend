using TZ.Backend.Console.Adapter.Interfaces;
using TZ.Backend.Services.Interfaces;
using TZ.Backend.Services.Runtime;

namespace TZ.Backend.Console.Adapter.Runtime
{
    public class BooksAdapter(string postgresConnectionString) : IProduct
    {
        private readonly IBooksService _booksService = new BooksService(postgresConnectionString);

        #region Use to fill database

        public async Task FillDatabase()
        {
            if (await _booksService.GetBooksCount() == 0)
            {
                await _booksService.AddAuthor(new()
                {
                    FullName = "J. K. Rowling"
                });

                await _booksService.AddAuthor(new()
                {
                    FullName = "Stace Krammer"
                });



                await _booksService.AddBook(new()
                {
                    AuthorId = 1,
                    Title = "Harry Potter and the Philosopher’s Stone",
                    Count = 10,
                    Date = new(1997, 6, 26)
                });

                await _booksService.AddBook(new()
                {
                    AuthorId = 1,
                    Title = "Harry Potter and the Chamber of Secrets",
                    Count = 10,
                    Date = new(1998, 7, 2)
                });

                await _booksService.AddBook(new()
                {
                    AuthorId = 1,
                    Title = "Harry Potter and the Prisoner of Azkaban",
                    Count = 10,
                    Date = new(1999, 7, 8)
                });

                await _booksService.AddBook(new()
                {
                    AuthorId = 1,
                    Title = "Harry Potter and the Goblet of Fire",
                    Count = 10,
                    Date = new(2000, 7, 8)
                });

                await _booksService.AddBook(new()
                {
                    AuthorId = 1,
                    Title = "Harry Potter and the Order of the Phoenix",
                    Count = 10,
                    Date = new(2003, 6, 21)
                });

                await _booksService.AddBook(new()
                {
                    AuthorId = 1,
                    Title = "Harry Potter and the Half-Blood Prince",
                    Count = 10,
                    Date = new(2005, 7, 16)
                });

                await _booksService.AddBook(new()
                {
                    AuthorId = 1,
                    Title = "Harry Potter and the Deathly Hallows",
                    Count = 10,
                    Date = new(2007, 7, 21)
                });




                await _booksService.AddBook(new()
                {
                    AuthorId = 2,
                    Title = "50 days before my suicide",
                    Count = 10,
                    Date = new(2016, 1, 1)
                });

                await _booksService.AddBook(new()
                {
                    AuthorId = 2,
                    Title = "I choose life",
                    Count = 10,
                    Date = new(2016, 1, 1)
                });

                await _booksService.AddBook(new()
                {
                    AuthorId = 2,
                    Title = "We are expired",
                    Count = 10,
                    Date = new(2016, 1, 1)
                });

                await _booksService.AddBook(new()
                {
                    AuthorId = 2,
                    Title = "Abyssal",
                    Count = 10,
                    Date = new(2019, 1, 1)
                });

                await _booksService.AddBook(new()
                {
                    AuthorId = 2,
                    Title = "I am madness",
                    Count = 10,
                    Date = new(2022, 1, 1)
                });

                await _booksService.AddBook(new()
                {
                    AuthorId = 2,
                    Title = "Youth will give you your first scars",
                    Count = 10,
                    Date = new(2023, 1, 1)
                });
            }
        }

        #endregion

        public async Task<string> Buy(string? id)
        {
            if (id == null || !int.TryParse(id, out var intId))
                return "Id have incorrect format";

            if (await _booksService.BuyBook(intId))
                return $"Complited, the book {id} was bought";

            return $"Error, book {id} not found or count < 0";
        }

        public async Task<string> Get(params string?[] flags)
        {
            string? title = null;
            int? authorId = null;
            DateOnly? date = null;
            string? orderBy = null;

            title = flags.Length > 0 && flags[0] != null ? flags[0] : null;

            if(flags.Length > 1 && flags[1] != null)
            {
                if (!int.TryParse(flags[1], out var temp))
                    return "Author id have incorrect format";

                authorId = temp;
            }

            if(flags.Length > 2 && flags[2] != null)
            {
                if (!DateOnly.TryParse(flags[2], out var temp))
                    return "Date have incorrect format";

                date = temp;
            }

            orderBy = flags.Length > 3 && flags[3] != null  ? flags[3] : null;

            if (orderBy != null && orderBy != "title" && orderBy != "author" && orderBy != "date" && orderBy != "count")
                return "Order by flag have incorrect format";

            var result = await _booksService.GetBooks(title, authorId, date, orderBy);

            if (result == null || result.Count() == 0)
                return "Books not found";

            string resultString = string.Empty;

            foreach(var book in result)
            {
                resultString += $"Id: {book.Id}, title: {book.Title}, autor: {book.AuthorName}, date: {book.Date}, count: {book.Count}\n";
            }

            return resultString;
        }

        public async Task<string> Restock(params string?[] flags)
        {
            int? id = null;
            int? count = null;

            if(flags.Length > 0 && flags[0] != null)
            {
                if (!int.TryParse(flags[0], out var temp))
                    return "Id have incorrect format";

                id = temp;
            }

            if(flags.Length > 1 && flags[1] != null)
            {
                if (!int.TryParse(flags[1], out var temp))
                    return "Count have incorrect format";

                count = temp;
            }

            if (await _booksService.RestockBooks(id, count))
                return "Restocked";

            return $"Error, book {id} not found";
        }
    }
}
