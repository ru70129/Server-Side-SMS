using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SMS.Services;
using SMS.ViewModels;

namespace SchoolManagementSystem2.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class SubjectsController : Controller
    {
        private ISubjectService _subjectService;

        public SubjectsController(ISubjectService subjectService)
        {
            _subjectService = subjectService;
        }

        public IActionResult Index(int pageNumber = 1, int pageSize = 10, string search = null, string sortBy = null, bool isActive = true)
        {
            var subjects = _subjectService.GetAll(pageNumber, pageSize, search, sortBy, isActive);
            ViewBag.Search = search;
            ViewBag.SortBy = sortBy;
            ViewBag.IsActive = isActive;
            return View(subjects);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateSubjectViewModel vm)
        {
            if (ModelState.IsValid)
            {
                vm.CreatedBy = User.Identity.Name ?? "System";
                await _subjectService.AddSubject(vm);
                return RedirectToAction("Index");
            }
            return View(vm);
        }

        [HttpGet]
        public IActionResult Details(int id)
        {
            var subject = _subjectService.GetById(id);
            if (subject == null) return NotFound();
            return View(subject);
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            var subject = _subjectService.GetById(id);
            if (subject == null) return NotFound();
            return View(subject);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(SubjectViewModel vm)
        {
            if (!ModelState.IsValid) return View(vm);

            var subjectExisting = _subjectService.GetById(vm.Id);
            if (subjectExisting == null) return NotFound();

            var currentUser = User.Identity?.Name ?? "System";
            if (!User.IsInRole("Admin") && !string.Equals(subjectExisting.CreatedBy, currentUser, StringComparison.OrdinalIgnoreCase))
            {
                return Forbid();
            }

            vm.UpdatedBy = currentUser;
            await _subjectService.UpdateSubject(vm);
            return RedirectToAction("Index");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var existing = _subjectService.GetById(id);
            if (existing == null) return NotFound();

            var currentUser = User.Identity?.Name ?? "System";
            if (!User.IsInRole("Admin") && !string.Equals(existing.CreatedBy, currentUser, StringComparison.OrdinalIgnoreCase))
            {
                return Forbid();
            }

            await _subjectService.DeleteSubject(id);
            return RedirectToAction("Index");
        }
    }
}