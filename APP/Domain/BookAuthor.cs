using CORE.APP.Domain;

namespace APP.Domain
{
    public class BookAuthor : Entity
    {
        public int BookId { get; set; }
        public Book Book { get; set; }
        public int AuthorId { get; set; }
        public Author Author { get; set; }
    }
}