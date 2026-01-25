using SMS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMS.ViewModels
{
    public class StudentViewModel
    {
        public int Id { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public DateTime DOB { get; set; }

        public DateTime DateOfJoin { get; set; }

        public bool Selected { get; set; }

        public string KeyId { get; set; }

        public bool SelectAll { get; set; }

        public bool IsActive { get; set; }

        public string CreatedBy { get; set; }

        public DateTime CreatedAt { get; set; }

        public string UpdatedBy { get; set; }

        public DateTime? UpdatedAt { get; set; }

        //public string Email { get; set; }

        public StudentViewModel(Student student)
        {
            Id = student.Id;
            FirstName = student.FirstName;
            LastName = student.LastName;
            DOB = student.DOB;
            DateOfJoin = student.DateOfJoin;
            KeyId = student.KeyId;
            IsActive = student.IsActive;
            CreatedBy = student.CreatedBy;
            CreatedAt = student.CreatedAt;
            UpdatedBy = student.UpdatedBy;
            UpdatedAt = student.UpdatedAt;

            //Email = student.Email;
        }


    }
}
