using CORE.APP.Models;

namespace APP.Models
{
    public class HomeResponse : Response
    {
        public string? Query { get; set; }
        public string SelectedCategory { get; set; } = "All";
        public string Sort { get; set; } = "newest";
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 12;
        public int TotalCount { get; set; }
        public int TotalPages { get; set; }
        public string Title { get; set; }
        public string Author { get; set; }

        public List<string> Genres { get; set; } = new();
        public List<string> Authors { get; set; } = new();
        public List<BookResponse> Books { get; set; } = new();
    }
}
