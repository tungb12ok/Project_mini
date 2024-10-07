using Project_mini.DTO;
using Project_mini.Models;

namespace Project_mini.Services;

public interface IDepartmentServices
{
    Task<DepartmentDTO?> AddDepartmentAsync(DepartmentDTO? department);

    Task<DepartmentDTO?> UpdateDepartmentAsync(DepartmentDTO department);

    Task<bool> DeleteDepartmentAsync(int id);

    Task<DepartmentDTO?> GetDepartmentByIdAsync(int id);

    Task<IEnumerable<DepartmentDTO?>> GetAllDepartmentsAsync();
    Task<List<DepartmentTreeDto>> GetChildren();
}