using Microsoft.AspNetCore.Mvc;
using SMS.ViewModels;

namespace SchoolManagementSystem2.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class StudentsController : Controller
    {
        // GET
        public IActionResult AddStudent()
        {
            return View();
        }

        // POST
        [HttpPost]
        public IActionResult AddStudent(CreateStudentViewModel vm)
        {
            if (!ModelState.IsValid)
            {
                return View(vm);
            }

            // later the service logic will be here
            return RedirectToAction("Index");
        }
    }
}
