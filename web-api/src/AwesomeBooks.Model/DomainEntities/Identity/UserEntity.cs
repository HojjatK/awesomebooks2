using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace AwesomeBooks.Model.DomainEntities.Identity
{
    public class UserEntity : IdentityUser<int>, IAggregateRoot
    {
        public Guid Uid { get; set; }

        [Timestamp]
        public byte[] Timestamp { get; set; }

        public virtual ICollection<UserRoleEntity> Roles { get; } = new List<UserRoleEntity>();

        public virtual ICollection<UserClaimEntity> Claims { get; } = new List<UserClaimEntity>();

        public virtual ICollection<UserLoginEntity> Logins { get; } = new List<UserLoginEntity>();

        public virtual ICollection<UserTokenEntity> Tokens { get; } = new List<UserTokenEntity>();
    }
}
