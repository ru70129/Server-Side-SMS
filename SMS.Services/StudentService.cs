using SMS.Models;
using SMS.Repositories;
using SMS.Utilities;
using SMS.ViewModels;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;

namespace SMS.Services
{
    public class StudentService : IStudentService
    {
        private IUnitOfWork _unitOfWork;
        private UserManager<ApplicationUser> _userManager;
        private RoleManager<IdentityRole> _roleManager;
        private readonly Microsoft.AspNetCore.Http.IHttpContextAccessor _httpContextAccessor;

        public StudentService(IUnitOfWork unitOfWork,
            UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager,
            Microsoft.AspNetCore.Http.IHttpContextAccessor httpContextAccessor)
        {
            _unitOfWork = unitOfWork;
            _userManager = userManager;
            _roleManager = roleManager;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task AddStudent(CreateStudentViewModel vm)
        {
            var student = new Student
            {
                FirstName = vm.FirstName,
                LastName = vm.LastName,
                DOB = vm.DOB,
                DateOfJoin = vm.DateOfJoin,
                KeyId = vm.KeyId,
                AllowedRoles = vm.AllowedRoles,
                IsActive = true,
                CreatedBy = vm.CreatedBy,
                CreatedAt = DateTime.Now
            };

            _unitOfWork.GenericRepository<Student>().Add(student);
            await _unitOfWork.SaveAsync();

            // Audit log
            var audit = new AuditLog
            {
                Action = "CREATE",
                EntityName = "Student",
                EntityId = student.Id,
                NewValues = JsonSerializer.Serialize(student),
                UserId = vm.CreatedBy
            };
            _unitOfWork.GenericRepository<AuditLog>().Add(audit);
            await _unitOfWork.SaveAsync();
        }

        public async Task UpdateStudent(StudentViewModel vm)
        {
            var student = _unitOfWork.GenericRepository<Student>().GetById(vm.Id);
            if (student == null) return;

            var oldValues = JsonSerializer.Serialize(student);

            student.FirstName = vm.FirstName;
            student.LastName = vm.LastName;
            student.DOB = vm.DOB;
            student.DateOfJoin = vm.DateOfJoin;
            student.KeyId = vm.KeyId;
            student.AllowedRoles = vm.AllowedRoles;
            student.UpdatedBy = vm.UpdatedBy;
            student.UpdatedAt = DateTime.Now;

            _unitOfWork.GenericRepository<Student>().Update(student);
            await _unitOfWork.SaveAsync();

            // Audit log
            var audit = new AuditLog
            {
                Action = "UPDATE",
                EntityName = "Student",
                EntityId = student.Id,
                OldValues = oldValues,
                NewValues = JsonSerializer.Serialize(student),
                UserId = vm.UpdatedBy
            };
            _unitOfWork.GenericRepository<AuditLog>().Add(audit);
            await _unitOfWork.SaveAsync();
        }

        public async Task DeleteStudent(int id)
        {
            var student = _unitOfWork.GenericRepository<Student>().GetById(id);
            if (student == null) return;

            var oldValues = JsonSerializer.Serialize(student);

            _unitOfWork.GenericRepository<Student>().Delete(student);
            await _unitOfWork.SaveAsync();

            // Audit log
            var audit = new AuditLog
            {
                Action = "DELETE",
                EntityName = "Student",
                EntityId = id,
                OldValues = oldValues,
                UserId = "System"
            };
            _unitOfWork.GenericRepository<AuditLog>().Add(audit);
            await _unitOfWork.SaveAsync();
        }

        public StudentViewModel GetById(int id)
        {
            var student = _unitOfWork.GenericRepository<Student>().GetById(id);
            if (student == null) return null;

            // Check AllowedRoles visibility
            var http = _httpContextAccessor?.HttpContext;
            if (!string.IsNullOrEmpty(student.AllowedRoles) && http != null)
            {
                var roles = student.AllowedRoles.Split(',').Select(r => r.Trim());
                var authorized = roles.Any(r => http.User?.IsInRole(r) == true);
                if (!authorized && !(http.User?.Identity?.IsAuthenticated == true))
                {
                    return null; // Hide from unauthorized users
                }
            }

            return new StudentViewModel(student);
        }

        public PagedResult<StudentViewModel> GetAll(int pageNumber, int pageSize, string search = null, string sortBy = null, bool isActive = true)
        {
            int totalCount = 0;
            List<StudentViewModel> vmList = new List<StudentViewModel>();
            try
            {
                var query = _unitOfWork.GenericRepository<Student>().GetAll().Where(s => s.IsActive == isActive);

                if (!string.IsNullOrEmpty(search))
                {
                    query = query.Where(s => s.FirstName.Contains(search) || s.LastName.Contains(search) || s.KeyId.Contains(search));
                }

                if (!string.IsNullOrEmpty(sortBy))
                {
                    switch (sortBy.ToLower())
                    {
                        case "firstname":
                            query = query.OrderBy(s => s.FirstName);
                            break;
                        case "lastname":
                            query = query.OrderBy(s => s.LastName);
                            break;
                        case "dob":
                            query = query.OrderBy(s => s.DOB);
                            break;
                        default:
                            query = query.OrderBy(s => s.Id);
                            break;
                    }
                }
                else
                {
                    query = query.OrderBy(s => s.Id);
                }

                totalCount = query.Count();
                var modelList = query.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();
                vmList = ConvertModelToViewModelList(modelList);
            }
            catch (Exception ex) { throw; }
            var result = new PagedResult<StudentViewModel>
            {
                Data = vmList,
                TotalItems = totalCount,
                PageNumber = pageNumber,
                PageSize = pageSize
            };
            return result;
        }

        public int GetAllStudents()
        {
            var totalCount = _unitOfWork.GenericRepository<Student>().GetAll().Count();
            return totalCount;
        }

        private List<StudentViewModel> ConvertModelToViewModelList(List<Student> modelList)
        {
            return modelList.Select(x => new StudentViewModel(x)).ToList();
        }
    }
}
