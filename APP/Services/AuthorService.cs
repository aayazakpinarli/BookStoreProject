using APP.Domain;
using APP.Models;
using CORE.APP.Models;
using CORE.APP.Services;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace APP.Services
{
    public class AuthorService : Service<Author>, IService<AuthorRequest, AuthorResponse>
    {
        public AuthorService(DbContext db) : base(db) { }

        protected override IQueryable<Author> Query(bool isNoTracking = true)
        {
            return base.Query(isNoTracking);
        }

        public List<AuthorResponse> List()
        {
            return Query(false)
                .Select(a => new AuthorResponse
                {
                    Id = a.Id,
                    Guid = a.Guid,
                    FullName = $"{a.FirstName} {a.LastName}",
                    FirstName = a.FirstName,
                    LastName = a.LastName
                })
                .ToList();
        }

        public AuthorResponse Item(int id)
        {
            var entity = Query().SingleOrDefault(a => a.Id == id);
            if (entity == null) return null;
            return new AuthorResponse
            {
                Id = entity.Id,
                Guid = entity.Guid,
                FullName = $"{entity.FirstName} {entity.LastName}",
                FirstName = entity.FirstName,
                LastName = entity.LastName
            };
        }

        public AuthorRequest Edit(int id)
        {
            var entity = Query(false).SingleOrDefault(a => a.Id == id);
            if (entity == null) return null;
            return new AuthorRequest
            {
                Id = entity.Id,
                FirstName = entity.FirstName,
                LastName = entity.LastName
            };
        }

        public CommandResponse Create(AuthorRequest request)
        {
            if (Query().Any(a => a.FirstName == request.FirstName.Trim() && a.LastName == request.LastName.Trim()))
                return Error("Author with the same full name exists!");

            var entity = new Author
            {
                FirstName = request.FirstName.Trim(),
                LastName = request.LastName.Trim()
            };
            Create(entity);
            return Success("Author created successfully.", entity.Id);
        }

        public CommandResponse Update(AuthorRequest request)
        {
            if (Query().Any(a => a.Id != request.Id && a.FirstName == request.FirstName.Trim() && a.LastName == request.LastName.Trim()))
                return Error("Author with the same full name exists!");

            var entity = Query(false).SingleOrDefault(a => a.Id == request.Id);
            if (entity == null) return Error("Author not found!");

            entity.FirstName = request.FirstName.Trim();
            entity.LastName = request.LastName.Trim();
            Update(entity);
            return Success("Author updated successfully.", entity.Id);
        }

        public CommandResponse Delete(int id)
        {
            var entity = Query(false).SingleOrDefault(a => a.Id == id);
            if (entity == null) return Error("Author not found!");
            Delete(entity);
            return Success("Author deleted successfully.", entity.Id);
        }
    }
}
