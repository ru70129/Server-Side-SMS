using System;
using System.ComponentModel.DataAnnotations;

namespace SMS.ViewModels
{
    public class CreateTeacherViewModel
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
        public DateTime DateofJoin { get; set; }

        [Required]
        [StringLength(20)]
        public string KeyId { get; set; }

        [StringLength(100)]
        public string Qualification { get; set; }

        public int YearOfEx { get; set; }

        /// <summary>
        /// Roles permitted to see/update this teacher record
        /// </summary>
        public string AllowedRoles { get; set; }

        public string CreatedBy { get; set; }
    }
}