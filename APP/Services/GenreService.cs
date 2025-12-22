using APP.Domain;
using APP.Models;
using CORE.APP.Models;
using CORE.APP.Services;
using Microsoft.EntityFrameworkCore;

namespace APP.Services
{
    public class GenreService : Service<Genre>, IService<GenreRequest, GenreResponse>
    {
        public GenreService(DbContext db) : base(db)
        {
        }

        protected override IQueryable<Genre> Query(bool isNoTracking = true)
        {
            return base.Query(isNoTracking).Include(g => g.BookGenres).ThenInclude(bg => bg.Book)
                .OrderBy(g => g.GenreName);
        }

        public List<GenreResponse> List()
        {
            return Query(false)
                .Select(g => new GenreResponse
                {
                    Id = g.Id,
                    Guid = g.Guid,
                    GenreName = g.GenreName,
                    BookCount = g.BookGenres.Count,
                    Books = string.Join(", ", g.BookGenres.Select(bg => bg.Book.BookName).ToList())
                })
                .ToList();
        }

        public GenreResponse Item(int id)
        {
            var entity = Query().SingleOrDefault(g => g.Id == id);
            if (entity is null) return null;

            return new GenreResponse
            {
                Id = entity.Id,
                Guid = entity.Guid,
                GenreName = entity.GenreName,
                BookCount = entity.BookGenres.Count
            };
        }

        public GenreRequest Edit(int id)
        {
            var entity = Query().SingleOrDefault(g => g.Id == id);
            if (entity is null) return null;

            return new GenreRequest
            {
                Id = entity.Id,
                GenreName = entity.GenreName
            };
        }

        public CommandResponse Create(GenreRequest request)
        {
            if (Query().Any(g => g.GenreName == request.GenreName.Trim()))
                return Error("Genre with the same name exists!");

            var entity = new Genre
            {
                GenreName = request.GenreName.Trim()
            };

            Create(entity);
            return Success("Genre created successfully.", entity.Id);
        }

        public CommandResponse Update(GenreRequest request)
        {
            if (Query().Any(g => g.Id != request.Id && g.GenreName == request.GenreName.Trim()))
                return Error("Genre with the same name exists!");

            var entity = Query(false).SingleOrDefault(g => g.Id == request.Id);
            if (entity is null) return Error("Genre not found!");

            entity.GenreName = request.GenreName.Trim();
            Update(entity);

            return Success("Genre updated successfully.", entity.Id);
        }

        public CommandResponse Delete(int id)
        {
            var entity = Query(false).SingleOrDefault(g => g.Id == id);
            if (entity is null) return Error("Genre not found!");

            var bookGenres = _db.Set<BookGenre>().Where(bg => bg.GenreId == id).ToList();
            if (bookGenres.Any())
            {
                _db.Set<BookGenre>().RemoveRange(bookGenres);
                _db.SaveChanges();
            }

            Delete(entity);
            return Success("Genre deleted successfully.", entity.Id);
        }
    }
}
