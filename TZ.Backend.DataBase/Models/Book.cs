namespace TZ.Backend.DataBase.Models
{
    public class Book
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public DateOnly Date { get; set; }
        public int Count { get; set; }

        public int AuthorId { get; set; }
        public Author? Author { get; set; }
    }
}
