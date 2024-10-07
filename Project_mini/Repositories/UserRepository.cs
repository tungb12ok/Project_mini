using Microsoft.EntityFrameworkCore;
using Project_mini.DTO;
using Project_mini.Helpers;
using Project_mini.Models;

namespace Project_mini.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly MiniProjectContext _context;

        public UserRepository(MiniProjectContext context)
        {
            _context = context;
        }

        public async Task<User> GetUserByIdAsync(int id) 
        {
            return await _context.Users
                .Include(x => x.Role)
                .Include(x => x.Department)
                .FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<IEnumerable<User>> GetAllUsersAsync() => await _context.Users.Include(x => x.Role)
            .Include(x => x.Department).ToListAsync();
        public async Task AddUserAsync(User user)
        {
            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();
        }
        public async Task UpdateUserAsync(User user)
        {
            _context.Users.Update(user);
            await _context.SaveChangesAsync();
        }
        public async Task DeleteUserAsync(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user != null)
            {
                _context.Users.Remove(user);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<User> GetUserByEmailAndPasswordAsync(LoginDTO loginDTO)
        {
            var user = await _context.Users
                .Include(x => x.Role)
                .FirstOrDefaultAsync(u => u.Email == loginDTO.email);
            if (user != null)
            {
                if (user.Password == Sha256HashHelper.ComputeSha256Hash(loginDTO.password))
                {
                    return user; 
                }
            }
            return null; 
        }

        public Task<List<User>> GetAllUserByDepartmentsAsync(int departmentId)
        {
            return _context.Users
                .Where(x => x.Department.Id == departmentId)
                .Include(x => x.Role)
                .Include(x => x.Department)
                .ToListAsync();
        }
    }
}
