using System;
using System.Collections.Generic;

namespace MyProApiDiplom.Models;

public partial class Chat
{
    public int Id { get; set; }

    public int? IdMassage { get; set; }

    public int? Client { get; set; }

    public int? Worker { get; set; }

    public virtual ICollection<All> Alls { get; set; } = new List<All>();
}
