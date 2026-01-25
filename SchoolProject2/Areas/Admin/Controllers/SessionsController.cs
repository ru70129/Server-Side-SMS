using Microsoft.AspNetCore.Mvc;
using SMS.Services;
using SMS.Utilities;
using SMS.ViewModels;

namespace SchoolManagementSystem2.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class SessionsController : Controller
    {
        private ISessionService _sessionService;
        public SessionsController(ISessionService sessionService)
        {
            _sessionService = sessionService;
        }
        public IActionResult Index(int pageNumber = 1, int pageSize = 10)
        {
            PagedResult<SessionViewModel> session = _sessionService.GetAll(pageNumber, pageSize);
            return View(session);
        }
        [HttpGet]
        public IActionResult Create()
        {
            CreateSessionViewModel vm = new CreateSessionViewModel();
            vm.Start = DateTime.Now.Year.ToString();
            var afterYear = DateTime.Now.AddYears(1);
            vm.End = afterYear.Year.ToString();
            return View(vm);
        }
        [HttpPost]
        public async Task<IActionResult> Create(CreateSessionViewModel vm)
        {
            if(vm == null) { return View(); }
            await _sessionService.Add(vm);
            return RedirectToAction("Index");
        }
    }
}
