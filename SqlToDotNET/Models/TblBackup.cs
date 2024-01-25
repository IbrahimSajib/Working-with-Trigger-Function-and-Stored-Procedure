using System;
using System.Collections.Generic;

namespace SqlToDotNET.Models;

public partial class TblBackup
{
    public int PersonId { get; set; }

    public string Name { get; set; } = null!;

    public DateOnly DateOfBirth { get; set; }
}
