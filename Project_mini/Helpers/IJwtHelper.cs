using Project_mini.Models;

namespace Project_mini.Helpers
{
    public interface IJwtHelper
    {
        string GenerateToken(User user);
    }

}
