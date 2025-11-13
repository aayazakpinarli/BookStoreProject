using CORE.APP.Domain;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace APP.Domain
{
    public class Role : Entity
    {
        [Required, StringLength(25)]
        public string RoleName { get; set; }

        // for users-roles many to many relationship
        public List<UserRole> UserRoles { get; set; } = new List<UserRole>();

        // since no need to update user property here, not going to access users from roles side

    }
}