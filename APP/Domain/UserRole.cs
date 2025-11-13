using APP.Domain;
using CORE.APP.Domain;
using System.Data;

namespace APP.Domain
{
    // for users-roles many to many relationship
    public class UserRole : Entity
    {
        public int UserId { get; set; } // FK: Users

        public User User { get; set; } // navigation property 

        public int RoleId { get; set; } // FK: Roles

        public Role Role { get; set; }
    }
}