using APP.Domain;
using APP.Models;
using CORE.APP.Services;

namespace APP.Services
{
    public class AuthorService : ServiceBase
    {
        private readonly Db _db;

        public AuthorService(Db db)
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
    }
}