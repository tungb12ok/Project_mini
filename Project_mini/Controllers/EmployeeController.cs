using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Project_mini.DTO;
using Project_mini.Helpers;
using Project_mini.Models;
using Project_mini.Repositories;
using Project_mini.Services;

namespace Project_mini.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeController : ControllerBase
    {
        private readonly IUserService _userService;

        public EmployeeController(IUserService userService)
        {
            _userService = userService;
        }

        [Authorize(Roles = "Manager, Admin")]
        [HttpGet("by-department/{departmentId}")]
        public async Task<IActionResult> GetEmployeeByDepartmentId(int departmentId)
        {
            var users = await _userService.GetAllUserByDepartmentsAsync(departmentId);

            return Ok(users);
        }

        [Authorize(Roles = "Manager, Admin")]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetEmployee(int id)
        {
            var user = await _userService.GetUserByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            return Ok(user);
        }

        // [Authorize(Roles = "Manager, Admin")]
        [HttpPost]
        public async Task<IActionResult> AddEmployee([FromBody] UserDto newUserDto)
        {
            if (newUserDto == null)
            {
                return BadRequest("Invalid employee data.");
            }

            var hashedPassword = Sha256HashHelper.ComputeSha256Hash(newUserDto.password);

            var newUser = new User
            {
                Name = newUserDto.Name,
                Email = newUserDto.Email,
                Password = hashedPassword,
                DepartmentId = newUserDto.DepartmentId,
                RoleId = 1
            };

            try
            {
                await _userService.AddUserAsync(newUser);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return StatusCode(500, "An error occurred while adding the employee.");
            }

            return CreatedAtAction(nameof(GetEmployee), new { id = newUser.Id }, newUser);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllEmployee()
        {
            var users = await _userService.GetAllUsersAsync();

            return Ok(users);
        }

        [Authorize(Roles = "Manager, Admin")]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateEmployee(int id, [FromBody] UserDto updatedUserDto)
        {
            if (id != updatedUserDto.Id)
            {
                return BadRequest("Employee ID mismatch");
            }

            var user = await _userService.GetUserByIdAsync(id);
            if (user == null)
            {
                return NotFound("User not found");
            }


            user.Name = updatedUserDto.Name;
            user.Email = updatedUserDto.Email;
            user.DepartmentId = updatedUserDto.DepartmentId;

            try
            {
                await _userService.UpdateUserAsync(user);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return StatusCode(500, "An error occurred while updating the employee.");
            }

            return NoContent();

        }
        [Authorize(Roles = "Manager, Admin")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEmployee(int id)
        {
            var user = await _userService.GetUserByIdAsync(id);
            if (user == null)
            {
                return NotFound($"Employee with ID {id} not found.");
            }

            try
            {
                await _userService.DeleteUserAsync(id);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return StatusCode(500, "An error occurred while deleting the employee.");
            }

            return NoContent(); // Trả về NoContent (204) sau khi xóa thành công
        }
    }
}