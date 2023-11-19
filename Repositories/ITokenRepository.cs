using Microsoft.AspNetCore.Identity;

namespace NZWalks.Repositories;

public interface ITokenRepository
{
    public string CreateJwtToken(IdentityUser user, List<string> roles);
}
