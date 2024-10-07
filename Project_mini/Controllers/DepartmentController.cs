using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Project_mini.DTO;
using Project_mini.Models;
using Project_mini.Services;

namespace Project_mini.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DepartmentController : ControllerBase
    {
        private readonly IDepartmentServices _departmentServices;

        public DepartmentController(IDepartmentServices departmentServices)
        {
            _departmentServices = departmentServices;
        }

        [Authorize(Roles = "Admin")]
        // POST: api/Department
        [HttpPost]
        public async Task<IActionResult> AddDepartment([FromBody] DepartmentDTO department)
        {
            if (department == null)
            {
                return BadRequest("Department cannot be null.");
            }

            var createdDepartment = await _departmentServices.AddDepartmentAsync(department);
            return CreatedAtAction(nameof(GetDepartmentById), new { id = createdDepartment?.Id }, createdDepartment);
        }

        [Authorize(Roles = "Admin")]
        // PUT: api/Department/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateDepartment(int id, [FromBody] DepartmentDTO department)
        {
            if (id != department.Id)
            {
                return BadRequest("Department ID mismatch.");
            }

            var updatedDepartment = await _departmentServices.UpdateDepartmentAsync(department);
            if (updatedDepartment == null)
            {
                return NotFound("Department not found.");
            }

            return Ok(updatedDepartment);
        }

        [Authorize(Roles = "Admin")]
        // DELETE: api/Department/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDepartment(int id)
        {
            var result = await _departmentServices.DeleteDepartmentAsync(id);
            if (!result)
            {
                return NotFound("Department not found.");
            }

            return NoContent();
        }

        [Authorize(Roles = "Admin")]
        // GET: api/Department/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetDepartmentById(int id)
        {
            var department = await _departmentServices.GetDepartmentByIdAsync(id);
            if (department == null)
            {
                return NotFound("Department not found.");
            }

            return Ok(department);
        }

        //[Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<IActionResult> GetAllDepartments()
        {
            var departments = await _departmentServices.GetAllDepartmentsAsync();
            return Ok(departments);
        }
        //[Authorize(Roles = "Admin")]
        [HttpGet("tree")]
        public async Task<IActionResult> GetDepartmentTree()
        {
            
            return Ok(_departmentServices.GetChildren());
        }
    }
}
