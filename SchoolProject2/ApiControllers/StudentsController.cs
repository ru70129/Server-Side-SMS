using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SMS.Services;
using SMS.ViewModels;
using System.Threading.Tasks;

namespace SchoolManagementSystem2.ApiControllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class StudentsController : ControllerBase
    {
        private IStudentService _studentService;

        public StudentsController(IStudentService studentService)
        {
            _studentService = studentService;
        }

        [HttpGet]
        public IActionResult Get(int pageNumber = 1, int pageSize = 10, string search = null, string sortBy = null)
        {
            var result = _studentService.GetAll(pageNumber, pageSize, search, sortBy, true);
            return Ok(result);
        }

        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            var student = _studentService.GetById(id);
            if (student == null || !student.IsActive) return NotFound();
            return Ok(student);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Post(CreateStudentViewModel vm)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            vm.CreatedBy = User.Identity.Name ?? "API";
            await _studentService.AddStudent(vm);
            return CreatedAtAction(nameof(Get), new { id = 0 }, vm); // Adjust as needed
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Put(int id, StudentViewModel vm)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var existing = _studentService.GetById(id);
            if (existing == null) return NotFound();
            vm.Id = id;
            vm.UpdatedBy = User.Identity.Name ?? "API";
            await _studentService.UpdateStudent(vm);
            return NoContent();
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int id)
        {
            var existing = _studentService.GetById(id);
            if (existing == null) return NotFound();
            await _studentService.DeleteStudent(id);
            return NoContent();
        }
    }
}