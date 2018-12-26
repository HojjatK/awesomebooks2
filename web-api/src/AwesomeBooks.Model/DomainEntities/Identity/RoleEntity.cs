using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace AwesomeBooks.Model.DomainEntities.Identity
{
    public class RoleEntity : IdentityRole<int>, IAggregateRoot
    {
        public Guid Uid { get; set; }

        [Timestamp]
        public byte[] Timestamp { get; set; }

        public virtual ICollection<RoleClaimEntity> Claims { get; } = new List<RoleClaimEntity>();
    }
}
