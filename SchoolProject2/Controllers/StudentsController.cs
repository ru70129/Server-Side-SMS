using Microsoft.AspNetCore.Mvc;
using SMS.Services;

namespace SchoolManagementSystem2.Controllers
{
    public class StudentsController : Controller
    {
        private IStudentService _studentService;

        public StudentsController(IStudentService studentService)
        {
            _studentService = studentService;
        }

        public IActionResult Index(int pageNumber = 1, int pageSize = 10, string search = null, string sortBy = null)
        {
            var students = _studentService.GetAll(pageNumber, pageSize, search, sortBy, true); // Only active
            ViewBag.Search = search;
            ViewBag.SortBy = sortBy;
            return View(students);
        }

        public IActionResult Details(int id)
        {
            var student = _studentService.GetById(id);
            if (student == null || !student.IsActive) return NotFound();
            return View(student);
        }
    }
}