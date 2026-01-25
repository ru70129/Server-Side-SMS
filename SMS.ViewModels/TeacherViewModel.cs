using SMS.Models;
using System;

namespace SMS.ViewModels
{
    public class TeacherViewModel
    {
        public TeacherViewModel() { }

        public TeacherViewModel(Teacher teacher)
        {
            Id = teacher.Id;
            FirstName = teacher.FirstName;
            LastName = teacher.LastName;
            DOB = teacher.DOB;
            DateofJoin = teacher.DateofJoin;
            KeyId = teacher.KeyId;
            Qualification = teacher.Qualification;
            YearOfEx = teacher.YearOfEx;
            IsActive = teacher.IsActive;
            CreatedBy = teacher.CreatedBy;
            CreatedAt = teacher.CreatedAt;
            UpdatedBy = teacher.UpdatedBy;
            UpdatedAt = teacher.UpdatedAt;
        }

        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime DOB { get; set; }
        public DateTime DateofJoin { get; set; }
        public string KeyId { get; set; }
        public string Qualification { get; set; }
        public int YearOfEx { get; set; }
        public bool IsActive { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedAt { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}