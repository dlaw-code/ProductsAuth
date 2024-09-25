using Assignment.API.Entity;

namespace Assignment.API.Service.Interface
{
    public interface IJwtTokenGenerator
    {
        string GenerateToken(ApplicationUser applicationUser);
    }
}
