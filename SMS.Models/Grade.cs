using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMS.Models
{
    public class Grade
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public bool IsActive { get; set; } = true;

        public string CreatedBy { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public string UpdatedBy { get; set; }
        public DateTime? UpdatedAt { get; set; }
        [NotMapped]
        public ICollection<AssignGrade> AssignGrade { get; set; } = new HashSet<AssignGrade>();

        [NotMapped]
        public ICollection<Enroll> Enrolls { get; set; } = new HashSet<Enroll>();

        public ICollection<GradeSubject> GradeSubjects { get; set; }


    }
}
