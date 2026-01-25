using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMS.Models
{
    public class Subject
    {
        public int Id { get; set; }

        [System.ComponentModel.DataAnnotations.Required]
        [System.ComponentModel.DataAnnotations.StringLength(100)]
        public string Name { get; set; }

        public bool IsActive { get; set; } = true;

        public string CreatedBy { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public string UpdatedBy { get; set; }

        public DateTime? UpdatedAt { get; set; }

        [NotMapped]
        public ICollection<GradeSubject> GradeSubjects { get; set; }

    }
}
