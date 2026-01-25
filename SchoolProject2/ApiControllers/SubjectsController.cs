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
    public class SubjectsController : ControllerBase
    {
        private ISubjectService _subjectService;

        public SubjectsController(ISubjectService subjectService)
        {
            _subjectService = subjectService;
        }

        [HttpGet]
        public IActionResult Get(int pageNumber = 1, int pageSize = 10, string search = null, string sortBy = null)
        {
            var result = _subjectService.GetAll(pageNumber, pageSize, search, sortBy, true);
            return Ok(result);
        }

        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            var subject = _subjectService.GetById(id);
            if (subject == null || !subject.IsActive) return NotFound();
            return Ok(subject);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Post(CreateSubjectViewModel vm)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            vm.CreatedBy = User.Identity.Name ?? "API";
            await _subjectService.AddSubject(vm);
            return CreatedAtAction(nameof(Get), new { id = 0 }, vm);
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Put(int id, SubjectViewModel vm)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var existing = _subjectService.GetById(id);
            if (existing == null) return NotFound();
            vm.Id = id;
            vm.UpdatedBy = User.Identity.Name ?? "API";
            await _subjectService.UpdateSubject(vm);
            return NoContent();
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int id)
        {
            var existing = _subjectService.GetById(id);
            if (existing == null) return NotFound();
            await _subjectService.DeleteSubject(id);
            return NoContent();
        }
    }
}