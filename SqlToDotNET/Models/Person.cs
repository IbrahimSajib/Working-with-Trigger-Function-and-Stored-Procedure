using System;
using System.Collections.Generic;

namespace SqlToDotNET.Models;

public partial class Person
{
    public int PersonId { get; set; }

    public string Name { get; set; } = null!;

    public DateOnly DateOfBirth { get; set; }
}
