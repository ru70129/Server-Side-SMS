using SMS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMS.ViewModels
{
    public class SessionViewModel
    {
        public int Id { get; set; }
        public string Start { get; set; } //2022-2023
        public string End { get; set; }
        public List<GradeViewModel> gradeViewModels { get; set; }
        public string UpdatedBy { get; set; }
        public string Combined
        {
            get
            {
                return Start + "-" + End;
            }
        }
        public SessionViewModel(Session model)
        {
            Id = model.Id;
            Start = model.Start;
            End = model.End;
            // Session model has no CreatedBy/Updated fields; can be extended if needed
        }
        public SessionViewModel()
        {

        }
    }
}
