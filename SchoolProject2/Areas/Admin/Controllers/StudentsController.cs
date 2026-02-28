using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using SMS.Services;
using SMS.ViewModels;

namespace SchoolManagementSystem2.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class StudentsController : Controller
    {
        private IStudentService _studentService;
        private IGradeService _gradeService;
        private ISessionService _sectionService;

        public StudentsController(IStudentService studentService, IGradeService gradeService, ISessionService sectionService)
        {
            _studentService = studentService;
            _gradeService = gradeService;
            _sectionService = sectionService;
        }

        public IActionResult Index(int pageNumber = 1, int pageSize = 10, string search = null, string sortBy = null, bool isActive = true)
        {
            ViewBag.Grades = new SelectList(_gradeService.GetAll(), "Id", "Name");
            ViewBag.sessions = new SelectList(_sectionService.GetAll(), "Id", "Combined");
            var students = _studentService.GetAll(pageNumber, pageSize, search, sortBy, isActive);
            ViewBag.Search = search;
            ViewBag.SortBy = sortBy;
            ViewBag.IsActive = isActive;
            return View(students);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateStudentViewModel vm)
        {
            if (ModelState.IsValid)
            {
                vm.CreatedBy = User.Identity.Name ?? "System";
                await _studentService.AddStudent(vm);
                return RedirectToAction("Index");
            }
            return View(vm);
        }

        [HttpGet]
        public IActionResult Details(int id)
        {
            var student = _studentService.GetById(id);
            if (student == null) return NotFound();
            return View(student);
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            var student = _studentService.GetById(id);
            if (student == null) return NotFound();
            return View(student);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(StudentViewModel vm)
        {
            if (!ModelState.IsValid) return View(vm);

            var existing = _studentService.GetById(vm.Id);
            if (existing == null) return NotFound();

            var currentUser = User.Identity?.Name ?? "System";
            if (!User.IsInRole("Admin") && !string.Equals(existing.CreatedBy, currentUser, StringComparison.OrdinalIgnoreCase))
            {
                return Forbid();
            }

            vm.UpdatedBy = currentUser;
            await _studentService.UpdateStudent(vm);
            return RedirectToAction("Index");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var existing = _studentService.GetById(id);
            if (existing == null) return NotFound();

            var currentUser = User.Identity?.Name ?? "System";
            if (!User.IsInRole("Admin") && !string.Equals(existing.CreatedBy, currentUser, StringComparison.OrdinalIgnoreCase))
            {
                return Forbid();
            }

            await _studentService.DeleteStudent(id);
            return RedirectToAction("Index");
        }
    }
}
