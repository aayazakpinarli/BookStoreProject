using APP.Domain;
using APP.Models;
using CORE.APP.Models;
using CORE.APP.Services;
using Microsoft.EntityFrameworkCore;

namespace APP.Services
{
    public class UserService : Service<User>, IService<UserRequest, UserResponse>
    {
        public UserService(DbContext db) : base(db)
        {
        }

        protected override IQueryable<User> Query(bool isNoTracking = true)
        {
            return base.Query(isNoTracking)
                .Include(u => u.UserRoles).ThenInclude(ur => ur.Role)
                .Include(u => u.Country)
                .Include(u => u.City)
                .OrderByDescending(u => u.IsActive).ThenBy(u => u.RegistrationDate).ThenBy(u => u.UserName);
        }

        public CommandResponse Create(UserRequest request)
        {
            if (Query().Any(u => u.UserName == request.UserName.Trim() && u.IsActive == request.IsActive))
                return Error("Active user with the same user name exists!");
            var entity = new User
            {
                UserName = request.UserName,
                Password = request.Password,
                FirstName = request.FirstName?.Trim(),
                LastName = request.LastName?.Trim(),
                Gender = request.Gender,
                BirthDate = request.BirthDate,
                RegistrationDate = DateTime.Now,
                IsActive = request.IsActive,
                Address = request.Address?.Trim(),
                RoleIds = request.RoleIds,
                CountryId = request.CountryId,
                CityId = request.CityId
            };
            Create(entity);
            return Success("User created successfully.", entity.Id);
        }

        public CommandResponse Delete(int id)
        {
            var entity = Query(false).SingleOrDefault(u => u.Id == id); 
            if (entity is null)
                return Error("User not found!");
            Delete(entity.UserRoles);
            Delete(entity);
            return Success("User deleted successfully.", entity.Id);
        }

        public UserRequest Edit(int id)
        {
            var entity = Query().SingleOrDefault(u => u.Id == id);
            if (entity is null)
                return null;
            return new UserRequest
            {
                Id = entity.Id,
                UserName = entity.UserName,
                Password = entity.Password,
                IsActive = entity.IsActive,
                FirstName = entity.FirstName,
                LastName = entity.LastName,
                Gender = entity.Gender,
                BirthDate = entity.BirthDate,
                Address = entity.Address,
                RoleIds = entity.RoleIds,
                CountryId = entity.CountryId,
                CityId = entity.CityId
            };
        }

        public UserResponse Item(int id)
        {
            var entity = Query().SingleOrDefault(u => u.Id == id);
            if (entity is null)
                return null;
            return new UserResponse
            {
                Id = entity.Id,
                Guid = entity.Guid,
                UserName = entity.UserName,
                Password = entity.Password,
                IsActive = entity.IsActive,
                FirstName = entity.FirstName,
                LastName = entity.LastName,
                Gender = entity.Gender,
                BirthDate = entity.BirthDate,
                RegistrationDate = entity.RegistrationDate,
                Address = entity.Address,
                RoleIds = entity.RoleIds,
                CountryId = entity.CountryId,
                CityId = entity.CityId,

                IsActiveF = entity.IsActive ? "Active" : "Inactive",
                FullName = entity.FirstName + " " + entity.LastName,
                GenderF = entity.Gender.ToString(),
                BirthDateF = entity.BirthDate.HasValue ? entity.BirthDate.Value.ToString("MM/dd/yyyy") : string.Empty,
                RegistrationDateF = entity.RegistrationDate.ToString("MM/dd/yyyy"),
                Roles = entity.UserRoles.Select(ur => ur.Role.RoleName).ToList(),
                Country = entity.Country != null ? entity.Country.CountryName : string.Empty,
                City = entity.City != null ? entity.City.CityName : string.Empty
            };
        }

        public List<UserResponse> List()
        {
            return Query().Select(u => new UserResponse
            {
                Id = u.Id,
                Guid = u.Guid,
                UserName = u.UserName,
                Password = u.Password,
                IsActive = u.IsActive,
                FirstName = u.FirstName,
                LastName = u.LastName,
                Gender = u.Gender,
                BirthDate = u.BirthDate,
                RegistrationDate = u.RegistrationDate,
                Address = u.Address,
                RoleIds = u.RoleIds,
                CountryId = u.CountryId,
                CityId = u.CityId,

                IsActiveF = u.IsActive ? "Active" : "Inactive",
                FullName = u.FirstName + " " + u.LastName,
                GenderF = u.Gender.ToString(), 
                BirthDateF = u.BirthDate.HasValue ? u.BirthDate.Value.ToString("MM/dd/yyyy") : string.Empty,
                RegistrationDateF = u.RegistrationDate.ToString("MM/dd/yyyy"),
                Roles = u.UserRoles.Select(ur => ur.Role.RoleName).ToList(),
                Country = u.Country != null ? u.Country.CountryName : string.Empty,
                City = u.City != null ? u.City.CityName : string.Empty
            }).ToList();
        }

        public CommandResponse Update(UserRequest request)
        {
            if (Query().Any(u => u.Id != request.Id && u.UserName == request.UserName.Trim() && u.IsActive == request.IsActive))
                return Error("Active user with the same user name exists!");
            var entity = Query(false).SingleOrDefault(u => u.Id == request.Id);
            if (entity is null)
                return Error("User not found!");
            Delete(entity.UserRoles);
            entity.UserName = request.UserName;
            entity.Password = request.Password;
            entity.FirstName = request.FirstName?.Trim();
            entity.LastName = request.LastName?.Trim();
            entity.Gender = request.Gender;
            entity.BirthDate = request.BirthDate;
            entity.IsActive = request.IsActive;
            entity.Address = request.Address?.Trim();
            entity.RoleIds = request.RoleIds;
            entity.CountryId = request.CountryId;
            entity.CityId = request.CityId;
            Update(entity);
            return Success("User updated successfully.", entity.Id);
        }

    }
}