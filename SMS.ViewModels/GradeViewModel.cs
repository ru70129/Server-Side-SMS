using SMS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMS.ViewModels
{
    public class GradeViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public bool IsActive { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedAt { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public GradeViewModel()
        {
        }
        public GradeViewModel(Grade grade)
        {
            Id = grade.Id;
            Name = grade.Name;
            IsActive = grade.IsActive;
            CreatedBy = grade.CreatedBy;
            CreatedAt = grade.CreatedAt;
            UpdatedBy = grade.UpdatedBy;
            UpdatedAt = grade.UpdatedAt;
        }

        //private List<SessionViewModel> ConvertModelToViewModelList(List<YearlySession> modelList);
        //{
        // return modelList.Select(x => new SessionViewModel(x)).ToList();
        //}

        public Grade Convert(GradeViewModel grade)
        {
            return new Grade { Id = this.Id, Name = this.Name, CreatedBy = this.CreatedBy, CreatedAt = this.CreatedAt, UpdatedBy = this.UpdatedBy, UpdatedAt = this.UpdatedAt, IsActive = this.IsActive };
        }
    }
}
    

