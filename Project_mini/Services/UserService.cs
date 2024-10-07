using Project_mini.Models;
using Project_mini.Repositories;

namespace Project_mini.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;

        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<User> GetUserByIdAsync(int id) => await _userRepository.GetUserByIdAsync(id);
        public async Task<IEnumerable<User>> GetAllUsersAsync() => await _userRepository.GetAllUsersAsync();

        public async Task<IEnumerable<User>> GetAllUserByDepartmentsAsync(int departmentId) =>
            await _userRepository.GetAllUserByDepartmentsAsync(departmentId);

        public async Task AddUserAsync(User user) => await _userRepository.AddUserAsync(user);
        public async Task UpdateUserAsync(User user) => await _userRepository.UpdateUserAsync(user);
        public async Task DeleteUserAsync(int id) => await _userRepository.DeleteUserAsync(id);
    }
}
