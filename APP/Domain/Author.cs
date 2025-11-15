using CORE.APP.Domain;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace APP.Domain
{
    public class Author : Entity
    {
        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }

        public List<BookAuthor> BookAuthor { get; set; } = new List<BookAuthor>();

        public List<UserBook> UserBooks { get; set; } = new List<UserBook>();

        [NotMapped]
        public List<int> BookIds
        {
            get => UserBooks.Select(userBookEntity => userBookEntity.BookId).ToList();
            set => UserBooks = value?.Select(bookId => new UserBook() { BookId = bookId }).ToList();
        }

    }
}