using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Project_mini.Models;

public partial class Department
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public int? ParentId { get; set; }

    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }
    [JsonIgnore]
    public virtual ICollection<Department> InverseParent { get; set; } = new List<Department>();
    [JsonIgnore]
    public virtual Department? Parent { get; set; }
    [JsonIgnore]
    public virtual ICollection<User> Users { get; set; } = new List<User>();
}
