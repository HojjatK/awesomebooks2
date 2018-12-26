using Microsoft.AspNetCore.Identity;

namespace AwesomeBooks.Model.DomainEntities.Identity
{
    public class RoleClaimEntity : IdentityRoleClaim<int>, IHasId
    {   
    }
}
