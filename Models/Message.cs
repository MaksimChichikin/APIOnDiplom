﻿using System;
using System.Collections.Generic;

namespace MyProApiDiplom.Models;

public partial class Message
{
    public int Id { get; set; }

    public int? SenderId { get; set; }

    public int? ReceiverId { get; set; }

    public string? Contents { get; set; }

    public DateTime? Timestamp { get; set; }
}
