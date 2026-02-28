using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SMS.Services;
using SMS.Utilities;
using SMS.ViewModels;

namespace SchoolManagementSystem2.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class GradesController : Controller
    {
        private IGradeService _gradeService;
        public GradesController(IGradeService gradeService)
        {
            _gradeService = gradeService;
        }
        [HttpGet]

        public IActionResult Index(int pageNumber = 1, int pageSize =10)
        {
            PagedResult<GradeViewModel> grade = _gradeService.GetAll(pageNumber, pageSize);
            return View(grade);
        }
        [HttpGet]
        public IActionResult AddGrade()
        { 
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> AddGrade(CreateGradeViewModel vm)
        {
            await _gradeService.Add(vm);
            return RedirectToAction("Index");
        }
    }
}
