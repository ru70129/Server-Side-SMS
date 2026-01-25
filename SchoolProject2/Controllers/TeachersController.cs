using Microsoft.AspNetCore.Mvc;
using SMS.Services;

namespace SchoolManagementSystem2.Controllers
{
    public class TeachersController : Controller
    {
        private ITeacherService _teacherService;

        public TeachersController(ITeacherService teacherService)
        {
            _teacherService = teacherService;
        }

        public IActionResult Index(int pageNumber = 1, int pageSize = 10, string search = null, string sortBy = null)
        {
            var teachers = _teacherService.GetAll(pageNumber, pageSize, search, sortBy, true); // Only active
            ViewBag.Search = search;
            ViewBag.SortBy = sortBy;
            return View(teachers);
        }

        public IActionResult Details(int id)
        {
            var teacher = _teacherService.GetById(id);
            if (teacher == null || !teacher.IsActive) return NotFound();
            return View(teacher);
        }
    }
}