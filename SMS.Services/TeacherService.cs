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
    public class TeacherService : ITeacherService
    {
        private IUnitOfWork _unitOfWork;
        private UserManager<ApplicationUser> _userManager;
        private RoleManager<IdentityRole> _roleManager;
    private readonly Microsoft.AspNetCore.Http.IHttpContextAccessor _httpContextAccessor;

        public TeacherService(IUnitOfWork unitOfWork,
            UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager,
            Microsoft.AspNetCore.Http.IHttpContextAccessor httpContextAccessor)
        {
            _unitOfWork = unitOfWork;
            _userManager = userManager;
            _roleManager = roleManager;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task AddTeacher(CreateTeacherViewModel vm)
        {
            var teacher = new Teacher
            {
                FirstName = vm.FirstName,
                LastName = vm.LastName,
                DOB = vm.DOB,
                DateofJoin = vm.DateofJoin,
                KeyId = vm.KeyId,
                Qualification = vm.Qualification,
                YearOfEx = vm.YearOfEx,
                IsActive = true,
                CreatedBy = vm.CreatedBy,
                CreatedAt = DateTime.Now,
                AllowedRoles = vm.AllowedRoles
            };

            _unitOfWork.GenericRepository<Teacher>().Add(teacher);
            await _unitOfWork.SaveAsync();

            // Audit log
            var audit = new AuditLog
            {
                Action = "CREATE",
                EntityName = "Teacher",
                EntityId = teacher.Id,
                NewValues = JsonSerializer.Serialize(teacher),
                UserId = vm.CreatedBy
            };
            _unitOfWork.GenericRepository<AuditLog>().Add(audit);
            await _unitOfWork.SaveAsync();
        }

        public async Task UpdateTeacher(TeacherViewModel vm)
        {
            var teacher = _unitOfWork.GenericRepository<Teacher>().GetById(vm.Id);
            if (teacher == null) return;

            var oldValues = JsonSerializer.Serialize(teacher);

            teacher.FirstName = vm.FirstName;
            teacher.LastName = vm.LastName;
            teacher.DOB = vm.DOB;
            teacher.DateofJoin = vm.DateofJoin;
            teacher.KeyId = vm.KeyId;
            teacher.Qualification = vm.Qualification;
            teacher.YearOfEx = vm.YearOfEx;
            teacher.AllowedRoles = vm.AllowedRoles;
            teacher.UpdatedBy = vm.UpdatedBy;
            teacher.UpdatedAt = DateTime.Now;

            _unitOfWork.GenericRepository<Teacher>().Update(teacher);
            await _unitOfWork.SaveAsync();

            // Audit log
            var audit = new AuditLog
            {
                Action = "UPDATE",
                EntityName = "Teacher",
                EntityId = teacher.Id,
                OldValues = oldValues,
                NewValues = JsonSerializer.Serialize(teacher),
                UserId = vm.UpdatedBy
            };
            _unitOfWork.GenericRepository<AuditLog>().Add(audit);
            await _unitOfWork.SaveAsync();
        }

        public async Task DeleteTeacher(int id)
        {
            var teacher = _unitOfWork.GenericRepository<Teacher>().GetById(id);
            if (teacher == null) return;

            var oldValues = JsonSerializer.Serialize(teacher);

            _unitOfWork.GenericRepository<Teacher>().Delete(teacher);
            await _unitOfWork.SaveAsync();

            // Audit log
            var audit = new AuditLog
            {
                Action = "DELETE",
                EntityName = "Teacher",
                EntityId = id,
                OldValues = oldValues,
                UserId = "System" // or get current user
            };
            _unitOfWork.GenericRepository<AuditLog>().Add(audit);
            await _unitOfWork.SaveAsync();
        }

        public TeacherViewModel? GetById(int id)
        {
            var teacher = _unitOfWork.GenericRepository<Teacher>().GetById(id);
            if (teacher == null) return null;

            // visibility check similar to StudentService
            var http = _httpContextAccessor?.HttpContext;
            if (!string.IsNullOrEmpty(teacher.AllowedRoles) && http != null)
            {
                var roles = teacher.AllowedRoles.Split(',').Select(r => r.Trim());
                var authorized = roles.Any(r => http.User?.IsInRole(r) == true);
                if (!authorized && !(http.User?.Identity?.IsAuthenticated == true))
                {
                    return null;
                }
            }

            return new TeacherViewModel(teacher);
        }

        public PagedResult<TeacherViewModel> GetAll(int pageNumber, int pageSize, string? search = null, string? sortBy = null, bool isActive = true)
        {
            int totalCount = 0;
            List<TeacherViewModel> vmList = new List<TeacherViewModel>();
            try
            {
                var query = _unitOfWork.GenericRepository<Teacher>().GetAll().Where(t => t.IsActive == isActive);

                if (!string.IsNullOrEmpty(search))
                {
                    query = query.Where(t => t.FirstName.Contains(search) || t.LastName.Contains(search) || t.KeyId.Contains(search));
                }

                if (!string.IsNullOrEmpty(sortBy))
                {
                    switch (sortBy.ToLower())
                    {
                        case "firstname":
                            query = query.OrderBy(t => t.FirstName);
                            break;
                        case "lastname":
                            query = query.OrderBy(t => t.LastName);
                            break;
                        case "dob":
                            query = query.OrderBy(t => t.DOB);
                            break;
                        default:
                            query = query.OrderBy(t => t.Id);
                            break;
                    }
                }
                else
                {
                    query = query.OrderBy(t => t.Id);
                }

                totalCount = query.Count();
                var modelList = query.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();
                vmList = ConvertModelToViewModelList(modelList);
            }
            catch { throw; }
            var result = new PagedResult<TeacherViewModel>
            {
                Data = vmList,
                TotalItems = totalCount,
                PageNumber = pageNumber,
                PageSize = pageSize
            };
            return result;
        }

        public int GetAllTeachers()
        {
            var totalCount = _unitOfWork.GenericRepository<Teacher>().GetAll().Count();
            return totalCount;
        }

        private List<TeacherViewModel> ConvertModelToViewModelList(List<Teacher> modelList)
        {
            return modelList.Select(x => new TeacherViewModel(x)).ToList();
        }
    }
}