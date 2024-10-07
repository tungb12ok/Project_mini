namespace Project_mini.DTO;

public class DepartmentDTO
{
    public int Id { get; set; }
    public string Name { get; set; }
    public int? ParentId { get; set; }
    public string? ParentDepartmentName { get; set; }
}