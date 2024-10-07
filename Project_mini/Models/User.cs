using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Project_mini.Models;

public partial class User
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string Password { get; set; } = null!;

    public int RoleId { get; set; }

    public int? DepartmentId { get; set; }
    [JsonIgnore]
    public virtual Department? Department { get; set; }
    [JsonIgnore]
    public virtual Role Role { get; set; } = null!;
}
