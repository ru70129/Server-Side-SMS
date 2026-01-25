using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using SMS.Services;
using SMS.ViewModels;

namespace SchoolManagementSystem2.Areas.Admin.Controllers
{
    [Area("Admin")]
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

        public IActionResult Index(int pageNumber=1, int pageSize=10)
        {
            ViewBag.Grades = new SelectList(_gradeService.GetAll(), "Id", "Name");
            ViewBag.sessions = new SelectList(_sectionService.GetAll(), "Id", "Combined");

            var students = _studentService.GetAll(pageNumber, pageSize); 
            return View(students);
        }

        [HttpGet]
        public async Task<IActionResult> AddStudent()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> AddStudent(CreateStudentViewModel vm)
        {
            await _studentService.AddStudent(vm);
            return RedirectToAction("Index");
        }
    }
}
