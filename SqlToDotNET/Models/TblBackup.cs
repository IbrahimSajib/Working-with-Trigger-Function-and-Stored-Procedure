using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace SqlToDotNET.Models;

public partial class TblBackup
{
    public int PersonId { get; set; }

    public string Name { get; set; } = null!;
    [DisplayFormat(DataFormatString = "{0:dd-MMM-yyyy}")]
    [DisplayName("Date of Birth")]
    public DateOnly DateOfBirth { get; set; }
}
