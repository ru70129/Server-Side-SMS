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
    public class TeachersController : ControllerBase
    {
        private ITeacherService _teacherService;

        public TeachersController(ITeacherService teacherService)
        {
            _teacherService = teacherService;
        }

        [HttpGet]
        public IActionResult Get(int pageNumber = 1, int pageSize = 10, string search = null, string sortBy = null)
        {
            var result = _teacherService.GetAll(pageNumber, pageSize, search, sortBy, true);
            return Ok(result);
        }

        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            var teacher = _teacherService.GetById(id);
            if (teacher == null || !teacher.IsActive) return NotFound();
            return Ok(teacher);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Post(CreateTeacherViewModel vm)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            vm.CreatedBy = User.Identity.Name ?? "API";
            await _teacherService.AddTeacher(vm);
            return CreatedAtAction(nameof(Get), new { id = 0 }, vm);
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Put(int id, TeacherViewModel vm)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var existing = _teacherService.GetById(id);
            if (existing == null) return NotFound();
            vm.Id = id;
            vm.UpdatedBy = User.Identity.Name ?? "API";
            await _teacherService.UpdateTeacher(vm);
            return NoContent();
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int id)
        {
            var existing = _teacherService.GetById(id);
            if (existing == null) return NotFound();
            await _teacherService.DeleteTeacher(id);
            return NoContent();
        }
    }
}