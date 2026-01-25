using SMS.Models;
using System;

namespace SMS.ViewModels
{
    public class SubjectViewModel
    {
        public SubjectViewModel() { }

        public SubjectViewModel(Subject subject)
        {
            Id = subject.Id;
            Name = subject.Name;
            IsActive = subject.IsActive;
            CreatedBy = subject.CreatedBy;
            CreatedAt = subject.CreatedAt;
            UpdatedBy = subject.UpdatedBy;
            UpdatedAt = subject.UpdatedAt;
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public bool IsActive { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedAt { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}