using System;
using System.Collections.Generic;
using WebApplication1.Models;

namespace WebApplication1.Models;

public partial class UserLogin
{
    public decimal Id { get; set; }

    public string? UserName { get; set; }

    public string? Passwordd { get; set; }

    public decimal? RoleId { get; set; }

    public decimal? CustomerId { get; set; }

    public virtual Customer? Customer { get; set; }

    public virtual Role? Role { get; set; }
}
