using SMS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMS.ViewModels
{
    public class CreateSessionViewModel
    {
        public string Start { get; set; }
        public string End { get; set; }
        public string CreatedBy { get; set; }

        public string UpdatedBy { get; set; }

        public Session Convert(CreateSessionViewModel vm)
        { 
            return new Session
            {
                Start = vm.Start,
                End = vm.End
            };
        }
    }
}
