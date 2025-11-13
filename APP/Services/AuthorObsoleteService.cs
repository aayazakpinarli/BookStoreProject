using APP.Domain;
using APP.Models;
using CORE.APP.Models;
using CORE.APP.Services;

namespace APP.Services
{
    public class AuthorObsoleteService : ServiceBase
    {
        private readonly Db _db;

        public AuthorObsoleteService(Db db)
        {
            _db = db;
        }

        public IQueryable<AuthorResponse> Query()
        {
            var query = _db.Authors.Select(authorEntity => new AuthorResponse
            {
                Id = authorEntity.Id,
                Guid = authorEntity.Guid,
                FirstName = authorEntity.FirstName,
                LastName = authorEntity.LastName
            });
            return query;
        }

        public CommandResponse Create(AuthorRequest request)
        {
            if (_db.Authors.Any(authorEntity => authorEntity.FirstName == request.FirstName.Trim()))
                return Error("Category with same title exists!");
            var entity = new Author
            {
                FirstName = request.FirstName.Trim(),
                LastName = request.LastName?.Trim(),
                Guid = Guid.NewGuid().ToString()
            };
            _db.Authors.Add(entity);
            _db.SaveChanges();
            return Success("Author created successfully", entity.Id);
        }

        public AuthorRequest Edit(int id)
        {
            var entity = _db.Authors.SingleOrDefault(a => a.Id == id);
            if (entity is null)
                return null;
            return new AuthorRequest
            {
                Id = entity.Id,
                FirstName = entity.FirstName,
                LastName = entity.LastName
            };
        }

        public CommandResponse Update(AuthorRequest request)
        {
            if (_db.Authors.Any(a => a.Id != request.Id && a.FirstName == request.FirstName.Trim() && a.LastName == request.LastName.Trim()) )
                return Error("Author with this first and last name exists!");
            var entity = _db.Authors.SingleOrDefault(a => a.Id == request.Id);
            if (entity is null)
                return Error("Author is not found!");
            entity.FirstName = request.FirstName?.Trim();
            entity.LastName = request.LastName?.Trim();
            _db.Authors.Update(entity);
            _db.SaveChanges();
            return Success("Author updated successfully.", entity.Id);
        }

        public CommandResponse Delete(int id)
        {
            var entity = _db.Authors.SingleOrDefault(a => a.Id == id);
            if (entity is null)
                return Error("Author could not found!");
            _db.Authors.Remove(entity);
            _db.SaveChanges();
            return Success("Author is deleted successfully.", entity.Id);
        }
    }
}