using SMS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMS.ViewModels
{
    public class CreateGradeViewModel
    {
        public string Name { get; set; }
        public string CreatedBy { get; set; }

        public string UpdatedBy { get; set; }

        public Grade Convert(CreateGradeViewModel vm)
        {
            return new Grade { Name = this.Name };
        }
    }


}
