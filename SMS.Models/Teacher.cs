using ServiceStack.DataAnnotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMS.Models
{
    public class Teacher
    {
        public int Id { get; set; }

        [System.ComponentModel.DataAnnotations.Required]
        [System.ComponentModel.DataAnnotations.StringLength(50)]
        public string FirstName { get; set; }

        [System.ComponentModel.DataAnnotations.Required]
        [System.ComponentModel.DataAnnotations.StringLength(50)]
        public string LastName { get; set; }

        [System.ComponentModel.DataAnnotations.Required]
        [System.ComponentModel.DataAnnotations.DataType(System.ComponentModel.DataAnnotations.DataType.Date)]
        public DateTime DOB { get; set; }

        [System.ComponentModel.DataAnnotations.Required]
        [System.ComponentModel.DataAnnotations.DataType(System.ComponentModel.DataAnnotations.DataType.Date)]
        public DateTime DateofJoin { get; set; }

        public bool Selected { get; set; }

        [System.ComponentModel.DataAnnotations.Required]
        [System.ComponentModel.DataAnnotations.StringLength(20)]
        [ServiceStack.DataAnnotations.Unique]
        public string KeyId { get; set; }

        [StringLength(100)]
        public string Qualification { get; set; }

        public int YearOfEx { get; set; }

        public bool IsActive { get; set; } = true;

        public string CreatedBy { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public string UpdatedBy { get; set; }

        public DateTime? UpdatedAt { get; set; }

        public ICollection<AssignGrade> AssignGrades { get; set; }

        public ICollection<TeacherSession> TeacherSessions { get; set; }

    }
}
