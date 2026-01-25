using SMS.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMS.ViewModels
{
    public class CreateStudentViewModel
    {
        [Required]
        [StringLength(50)]
        public string FirstName { get; set; }

        [Required]
        [StringLength(50)]
        public string LastName { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime DOB { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime DateOfJoin { get; set; } = DateTime.Now;

        public bool Selected { get; set; }

        [Required]
        [StringLength(20)]
        public string KeyId { get; set; }

        public string UserName { get; set; }

        public string Email { get; set; }

        public string Password { get; set; }

        public string CreatedBy { get; set; }

        public Student ConvertModel(CreateStudentViewModel student)
        {
            return new Student
            {
                FirstName = student.FirstName,
                LastName = student.LastName,
                DOB = student.DOB,
                DateOfJoin = student.DateOfJoin,
                KeyId = student.KeyId,
            };
        }

    }
}
