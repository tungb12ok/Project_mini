namespace Project_mini.DTO;

public class DepartmentTreeDto
{
    public int Id { get; set; }
    public string Name { get; set; }
    public int? ParentId { get; set; }
    public string ParentDepartmentName { get; set; } 
    public List<DepartmentTreeDto> Children { get; set; } = new List<DepartmentTreeDto>();
}
