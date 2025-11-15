using CORE.APP.Domain;

namespace APP.Domain
{
    public class UserBook : Entity
    {
        public int BookId { get; set; }
        public Book Book { get; set; }
        public int UserId { get; set; }
        public User User { get; set; }
    }
}