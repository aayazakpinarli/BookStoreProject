using APP.Domain;
using APP.Models;
using CORE.APP.Models;
using CORE.APP.Services;
using Microsoft.EntityFrameworkCore;

namespace APP.Services
{
    public class RoleService : Service<Role>, IService<RoleRequest, RoleResponse>
    {
        public RoleService(DbContext db) : base(db)
        {
        }

        protected override IQueryable<Role> Query(bool isNoTracking = true)
        {
            return base.Query(isNoTracking)
                       .OrderBy(r => r.RoleName);
        }

        public List<RoleResponse> List()
        {
            return Query(false)
                .Select(r => new RoleResponse
                {
                    Id = r.Id,
                    Guid = r.Guid,
                    Name = r.RoleName,
                    UserCount = r.UserRoles.Count,
                    Users = string.Join(", ", r.UserRoles.Select(ur => ur.User.UserName))
                })
                .ToList();
        }

        public RoleResponse Item(int id)
        {
            var entity = Query().SingleOrDefault(r => r.Id == id);
            if (entity is null) return null;

            return new RoleResponse
            {
                Id = entity.Id,
                Guid = entity.Guid,
                Name = entity.RoleName,
                UserCount = entity.UserRoles.Count,
                Users = string.Join(", ", entity.UserRoles.Select(ur => ur.User.UserName))
            };
        }

        public RoleRequest Edit(int id)
        {
            var entity = Query().SingleOrDefault(r => r.Id == id);
            if (entity is null) return null;

            return new RoleRequest
            {
                Id = entity.Id,
                RoleName = entity.RoleName
            };
        }

        public CommandResponse Create(RoleRequest request)
        {
            if (Query().Any(r => r.RoleName == request.RoleName.Trim()))
                return Error("Role with the same name exists!");

            var entity = new Role
            {
                RoleName = request.RoleName.Trim()
            };

            Create(entity);
            return Success("Role created successfully.", entity.Id);
        }

        public CommandResponse Update(RoleRequest request)
        {
            if (Query().Any(r => r.Id != request.Id && r.RoleName == request.RoleName.Trim()))
                return Error("Role with the same name exists!");

            var entity = Query(false).SingleOrDefault(r => r.Id == request.Id);
            if (entity is null) return Error("Role not found!");

            entity.RoleName = request.RoleName.Trim();
            Update(entity);

            return Success("Role updated successfully.", entity.Id);
        }

        public CommandResponse Delete(int id)
        {
            var entity = Query(false).SingleOrDefault(r => r.Id == id);
            if (entity is null) return Error("Role not found!");

            var userRoles = _db.Set<UserRole>().Where(ur => ur.RoleId == id).ToList();
            if (userRoles.Any())
            {
                _db.Set<UserRole>().RemoveRange(userRoles);
                _db.SaveChanges();
            }

            Delete(entity);
            return Success("Role deleted successfully.", entity.Id);
        }
    }
}
