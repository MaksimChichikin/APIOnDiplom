using System;
using System.Collections.Generic;

namespace MyProApiDiplom.Models;

public partial class User
{
    public int Id { get; set; }

    public string? FullName { get; set; }

    public string? Email { get; set; }

    public string? Password { get; set; }

    public int? IdRole { get; set; }

    public virtual ICollection<All> Alls { get; set; } = new List<All>();

    public virtual Role? IdRoleNavigation { get; set; }
}
