using Project_mini.DTO;
using Project_mini.Models;

namespace Project_mini.Repositories
{
    public interface IUserRepository
    {
        Task<User> GetUserByIdAsync(int id);
        Task<IEnumerable<User>> GetAllUsersAsync();
        Task AddUserAsync(User user);
        Task UpdateUserAsync(User user);
        Task DeleteUserAsync(int id);
        Task<User> GetUserByEmailAndPasswordAsync(LoginDTO loginDTO); 
        Task<List<User>> GetAllUserByDepartmentsAsync(int departmentId);

    }
}
