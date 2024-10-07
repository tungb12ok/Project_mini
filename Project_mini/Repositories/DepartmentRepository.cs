using Project_mini.Models;
using Microsoft.EntityFrameworkCore;
using Project_mini.DTO;

namespace Project_mini.Repositories
{
    public class DepartmentRepository : IDepartmentRepository
    {
        private readonly MiniProjectContext _context;

        public DepartmentRepository(MiniProjectContext context)
        {
            _context = context;
        }

        public async Task<DepartmentDTO?> AddAsync(DepartmentDTO? dto)
        {
            try
            {
                if (dto == null)
                {
                    throw new ArgumentNullException(nameof(dto));
                }

                var entity = new Department
                {
                    Name = dto.Name,
                    ParentId = dto.ParentId
                };

                await _context.Departments.AddAsync(entity);
                await _context.SaveChangesAsync();

                return new DepartmentDTO
                {
                    Id = entity.Id,
                    Name = entity.Name,
                    ParentId = entity.ParentId,
                    ParentDepartmentName = entity.Parent?.Name
                };
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                throw;
            }
        }

        public async Task<DepartmentDTO?> UpdateAsync(DepartmentDTO dto)
        {
            try
            {
                var entity = await _context.Departments.FindAsync(dto.Id);
                if (entity == null)
                {
                    throw new Exception("Department not found.");
                }

                entity.Name = dto.Name;
                entity.ParentId = dto.ParentId;

                _context.Departments.Update(entity);
                await _context.SaveChangesAsync();

                return new DepartmentDTO
                {
                    Id = entity.Id,
                    Name = entity.Name,
                    ParentId = entity.ParentId,
                    ParentDepartmentName = entity.Parent?.Name
                };
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                throw;
            }
        }

        public async Task<bool> DeleteAsync(int id)
        {
            try
            {
                var department = await _context.Departments.FindAsync(id);
                if (department == null)
                {
                    return false;
                }

                _context.Departments.Remove(department);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                throw;
            }
        }

        public async ValueTask<DepartmentDTO?> GetByIdAsync(int id)
        {
            try
            {
                var department = await _context.Departments
                    .Include(d => d.Parent)
                    .FirstOrDefaultAsync(d => d.Id == id);

                if (department == null)
                {
                    return null;
                }

                return new DepartmentDTO
                {
                    Id = department.Id,
                    Name = department.Name,
                    ParentId = department.ParentId,
                    ParentDepartmentName = department.Parent?.Name
                };
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                throw;
            }
        }

        public async Task<IEnumerable<DepartmentDTO>> GetAllAsync()
        {
            try
            {
                return await _context.Departments
                    .Include(d => d.Parent)
                    .Select(d => new DepartmentDTO
                    {
                        Id = d.Id,
                        Name = d.Name,
                        ParentId = d.ParentId,
                        ParentDepartmentName = d.Parent != null ? d.Parent.Name : null
                    })
                    .ToListAsync();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                throw;
            }
        }
    }

}
