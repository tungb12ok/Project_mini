using Project_mini.Models;

namespace Project_mini.Services
{
    public interface IUserService
    {
        Task<User> GetUserByIdAsync(int id);
        Task<IEnumerable<User>> GetAllUsersAsync();
        Task<IEnumerable<User>> GetAllUserByDepartmentsAsync(int departmentId);
        Task AddUserAsync(User user);
        Task UpdateUserAsync(User user);
        Task DeleteUserAsync(int id);
    }

}
