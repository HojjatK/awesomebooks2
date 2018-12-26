using Microsoft.AspNetCore.Identity;

namespace AwesomeBooks.Model.DomainEntities.Identity
{
    public class UserClaimEntity : IdentityUserClaim<int>, IHasId
    {   
    }
}
