using Project_mini.DTO;
using Project_mini.Models;

namespace Project_mini.Repositories
{
    public interface IDepartmentRepository
    {
        Task<DepartmentDTO> AddAsync(DepartmentDTO dto);

        Task<DepartmentDTO> UpdateAsync(DepartmentDTO dto);

        Task<bool> DeleteAsync(int id);

        ValueTask<DepartmentDTO?> GetByIdAsync(int id);

        Task<IEnumerable<DepartmentDTO>> GetAllAsync();

    }
}