using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using SMS.Services;
using SMS.ViewModels;

namespace SchoolManagementSystem2.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class TeachersController : Controller
    {
        private ITeacherService _teacherService;

        public TeachersController(ITeacherService teacherService)
        {
            _teacherService = teacherService;
        }

        public IActionResult Index(int pageNumber = 1, int pageSize = 10, string search = null, string sortBy = null, bool isActive = true)
        {
            var teachers = _teacherService.GetAll(pageNumber, pageSize, search, sortBy, isActive);
            ViewBag.Search = search;
            ViewBag.SortBy = sortBy;
            ViewBag.IsActive = isActive;
            return View(teachers);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateTeacherViewModel vm)
        {
            if (ModelState.IsValid)
            {
                vm.CreatedBy = User.Identity.Name ?? "System";
                await _teacherService.AddTeacher(vm);
                return RedirectToAction("Index");
            }
            return View(vm);
        }

        [HttpGet]
        public IActionResult Details(int id)
        {
            var teacher = _teacherService.GetById(id);
            if (teacher == null) return NotFound();
            return View(teacher);
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            var teacher = _teacherService.GetById(id);
            if (teacher == null) return NotFound();
            return View(teacher);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(TeacherViewModel vm)
        {
            if (!ModelState.IsValid) return View(vm);

            var existing = _teacherService.GetById(vm.Id);
            if (existing == null) return NotFound();

            var currentUser = User.Identity?.Name ?? "System";
            if (!User.IsInRole("Admin") && !string.Equals(existing.CreatedBy, currentUser, StringComparison.OrdinalIgnoreCase))
            {
                return Forbid();
            }

            vm.UpdatedBy = currentUser;
            await _teacherService.UpdateTeacher(vm);
            return RedirectToAction("Index");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var existing = _teacherService.GetById(id);
            if (existing == null) return NotFound();

            var currentUser = User.Identity?.Name ?? "System";
            if (!User.IsInRole("Admin") && !string.Equals(existing.CreatedBy, currentUser, StringComparison.OrdinalIgnoreCase))
            {
                return Forbid();
            }

            await _teacherService.DeleteTeacher(id);
            return RedirectToAction("Index");
        }
    }
}