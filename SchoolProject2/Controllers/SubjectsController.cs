using Microsoft.AspNetCore.Mvc;
using SMS.Services;

namespace SchoolManagementSystem2.Controllers
{
    public class SubjectsController : Controller
    {
        private ISubjectService _subjectService;

        public SubjectsController(ISubjectService subjectService)
        {
            _subjectService = subjectService;
        }

        public IActionResult Index(int pageNumber = 1, int pageSize = 10, string search = null, string sortBy = null)
        {
            var subjects = _subjectService.GetAll(pageNumber, pageSize, search, sortBy, true); // Only active
            ViewBag.Search = search;
            ViewBag.SortBy = sortBy;
            return View(subjects);
        }

        public IActionResult Details(int id)
        {
            var subject = _subjectService.GetById(id);
            if (subject == null || !subject.IsActive) return NotFound();
            return View(subject);
        }
    }
}