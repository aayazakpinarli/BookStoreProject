using CORE.APP.Domain;
using Microsoft.EntityFrameworkCore;

namespace CORE.APP.Services
{
    public abstract class Service<TEntity> : ServiceBase, IDisposable where TEntity : Entity, new()
    {
        protected readonly DbContext _db;


        protected Service(DbContext db)
        {
            _db = db;
        }


        protected virtual IQueryable<TEntity> Query(bool isNoTracking = true)
        {
            return isNoTracking ? _db.Set<TEntity>().AsNoTracking() : _db.Set<TEntity>();
        }


        protected virtual int Save() => _db.SaveChanges();


        protected void Create(TEntity entity, bool save = true)
        {
            entity.Guid = Guid.NewGuid().ToString(); // generate a new guid for the entity that will be inserted
            _db.Set<TEntity>().Add(entity);
            if (save)
                Save();
        }


        protected void Update(TEntity entity, bool save = true)
        {
            _db.Set<TEntity>().Update(entity);
            if (save)
                Save();
        }


        protected void Delete(TEntity entity, bool save = true)
        {
            _db.Set<TEntity>().Remove(entity);
            if (save)
                Save();
        }


        protected virtual async Task<int> Save(CancellationToken cancellationToken) => await _db.SaveChangesAsync(cancellationToken);


        protected async Task Create(TEntity entity, CancellationToken cancellationToken, bool save = true)
        {
            entity.Guid = Guid.NewGuid().ToString(); 
            _db.Set<TEntity>().Add(entity);
            if (save)
                await Save(cancellationToken);
        }


        protected async Task Update(TEntity entity, CancellationToken cancellationToken, bool save = true)
        {
            _db.Set<TEntity>().Update(entity);
            if (save)
                await Save(cancellationToken);
        }


        protected async Task Delete(TEntity entity, CancellationToken cancellationToken, bool save = true)
        {
            _db.Set<TEntity>().Remove(entity);
            if (save)
                await Save(cancellationToken);
        }


        public IQueryable<TRelationalEntity> Query<TRelationalEntity>() where TRelationalEntity : Entity, new()
        {
            return _db.Set<TRelationalEntity>().AsNoTracking();
        }


        protected void Delete<TRelationalEntity>(List<TRelationalEntity> relationalEntities) where TRelationalEntity : Entity, new()
        {
            _db.Set<TRelationalEntity>().RemoveRange(relationalEntities);
        }


        public void Dispose()
        {
            _db.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}