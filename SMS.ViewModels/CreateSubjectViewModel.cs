using System.ComponentModel.DataAnnotations;

namespace SMS.ViewModels
{
    public class CreateSubjectViewModel
    {
        [Required]
        [StringLength(100)]
        public string Name { get; set; }

        public string CreatedBy { get; set; }
    }
}