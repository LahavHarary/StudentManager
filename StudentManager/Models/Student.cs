using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace StudentManager.Models
{
    public class Student
    {
        // Change later - need to lose the ID
        [JsonIgnore]
        public int Id {get;set;}
        public string FirstName { get; set; }
        public string LastName { get; set; } 
        public int Age { get; set; }
        public double AverageGrade { get; set; }
        public string SchoolName { get; set; }
        public string SchoolAddress { get; set; }

    }
}
