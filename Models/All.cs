using System;
using System.Collections.Generic;

namespace MyProApiDiplom.Models;

public partial class All
{
    public int Id { get; set; }

    public int? IdUser { get; set; }

    public virtual User? IdUserNavigation { get; set; }
}
