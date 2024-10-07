using Project_mini.DTO;
using Project_mini.Models;
using Project_mini.Repositories;

namespace Project_mini.Services
{
    public class DepartmentServices : IDepartmentServices
    {
        private readonly IDepartmentRepository _departmentRepository;

        public DepartmentServices(IDepartmentRepository departmentRepository)
        {
            _departmentRepository = departmentRepository;
        }

        public async Task<DepartmentDTO?> AddDepartmentAsync(DepartmentDTO? department)
        {
            if (department == null)
            {
                throw new ArgumentNullException(nameof(department));
            }

            return await _departmentRepository.AddAsync(department);
        }

        public async Task<DepartmentDTO?> UpdateDepartmentAsync(DepartmentDTO department)
        {
            var existingDepartment = await _departmentRepository.GetByIdAsync(department.Id);
            if (existingDepartment == null)
            {
                throw new Exception("Department not found.");
            }

            return await _departmentRepository.UpdateAsync(department);
        }

        public async Task<bool> DeleteDepartmentAsync(int id)
        {
            return await _departmentRepository.DeleteAsync(id);
        }

        public async Task<DepartmentDTO?> GetDepartmentByIdAsync(int id)
        {
            return await _departmentRepository.GetByIdAsync(id);
        }

        public async Task<IEnumerable<DepartmentDTO?>> GetAllDepartmentsAsync()
        {
            return await _departmentRepository.GetAllAsync();
        }
        public async Task<List<DepartmentTreeDto>> GetChildren()
        {
            var departments = await GetAllDepartmentsAsync();

            var departmentDict = departments
                .ToDictionary(d => d.Id, d => new DepartmentTreeDto
                {
                    Id = d.Id,
                    Name = d.Name,
                    ParentId = d.ParentId,
                    ParentDepartmentName = departments.FirstOrDefault(p => p.Id == d.ParentId)?.Name,
                    Children = new List<DepartmentTreeDto>()
                });

            var tree = new List<DepartmentTreeDto>();

            foreach (var department in departmentDict.Values)
            {
                if (department.ParentId == null)
                {
                    tree.Add(department);
                }
                else if (departmentDict.ContainsKey(department.ParentId.Value))
                {
                    departmentDict[department.ParentId.Value].Children.Add(department);
                }
            }

            return tree;
        }

    }
}