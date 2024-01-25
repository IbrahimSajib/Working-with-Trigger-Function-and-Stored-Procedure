using System;
using System.Collections.Generic;

namespace SqlToDotNET.Models;

public partial class PersonAudit
{
    public int Id { get; set; }

    public string? AuditData { get; set; }
}
