using SqlToDotNET.Models;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace SqlToDotNET.ViewModels
{
    //public class PersonWithAgeVM:Person
    //{
    //    public int Age { get; set; }
    //}
    public class PersonWithAgeVM
    {
        [Key]
        public int PersonId { get; set; }

        public string Name { get; set; } = null!;

        [DisplayFormat(DataFormatString = "{0:dd-MMM-yyyy}")]
        [DisplayName("Date of Birth")]
        public DateOnly DateOfBirth { get; set; }
        public int Age { get; set; }
    }
}
