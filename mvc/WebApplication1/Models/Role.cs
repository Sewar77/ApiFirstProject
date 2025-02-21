using WebApplication1.Models;
using System;
using System.Collections.Generic;

namespace WebApplication1.Models;

public partial class Role
{
    public decimal RoleId { get; set; }

    public string? RoleName { get; set; }

    public virtual ICollection<UserLogin> UserLogins { get; set; } = new List<UserLogin>();
}
