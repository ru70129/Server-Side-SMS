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
    public class SubjectService : ISubjectService
    {
        private IUnitOfWork _unitOfWork;
        private UserManager<ApplicationUser> _userManager;
        private RoleManager<IdentityRole> _roleManager;

        public SubjectService(IUnitOfWork unitOfWork,
            UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager)
        {
            _unitOfWork = unitOfWork;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public async Task AddSubject(CreateSubjectViewModel vm)
        {
            var subject = new Subject
            {
                Name = vm.Name,
                IsActive = true,
                CreatedBy = vm.CreatedBy,
                CreatedAt = DateTime.Now
            };

            _unitOfWork.GenericRepository<Subject>().Add(subject);
            await _unitOfWork.SaveAsync();

            // Audit log
            var audit = new AuditLog
            {
                Action = "CREATE",
                EntityName = "Subject",
                EntityId = subject.Id,
                NewValues = JsonSerializer.Serialize(subject),
                UserId = vm.CreatedBy
            };
            _unitOfWork.GenericRepository<AuditLog>().Add(audit);
            await _unitOfWork.SaveAsync();
        }

        public async Task UpdateSubject(SubjectViewModel vm)
        {
            var subject = _unitOfWork.GenericRepository<Subject>().GetById(vm.Id);
            if (subject == null) return;

            var oldValues = JsonSerializer.Serialize(subject);

            subject.Name = vm.Name;
            subject.UpdatedBy = vm.UpdatedBy;
            subject.UpdatedAt = DateTime.Now;

            _unitOfWork.GenericRepository<Subject>().Update(subject);
            await _unitOfWork.SaveAsync();

            // Audit log
            var audit = new AuditLog
            {
                Action = "UPDATE",
                EntityName = "Subject",
                EntityId = subject.Id,
                OldValues = oldValues,
                NewValues = JsonSerializer.Serialize(subject),
                UserId = vm.UpdatedBy
            };
            _unitOfWork.GenericRepository<AuditLog>().Add(audit);
            await _unitOfWork.SaveAsync();
        }

        public async Task DeleteSubject(int id)
        {
            var subject = _unitOfWork.GenericRepository<Subject>().GetById(id);
            if (subject == null) return;

            var oldValues = JsonSerializer.Serialize(subject);

            _unitOfWork.GenericRepository<Subject>().Delete(subject);
            await _unitOfWork.SaveAsync();

            // Audit log
            var audit = new AuditLog
            {
                Action = "DELETE",
                EntityName = "Subject",
                EntityId = id,
                OldValues = oldValues,
                UserId = "System"
            };
            _unitOfWork.GenericRepository<AuditLog>().Add(audit);
            await _unitOfWork.SaveAsync();
        }

        public SubjectViewModel GetById(int id)
        {
            var subject = _unitOfWork.GenericRepository<Subject>().GetById(id);
            return subject == null ? null : new SubjectViewModel(subject);
        }

        public PagedResult<SubjectViewModel> GetAll(int pageNumber, int pageSize, string? search = null, string? sortBy = null, bool isActive = true)
        {
            int totalCount = 0;
            List<SubjectViewModel> vmList = new List<SubjectViewModel>();
            try
            {
                var query = _unitOfWork.GenericRepository<Subject>().GetAll().Where(s => s.IsActive == isActive);

                if (!string.IsNullOrEmpty(search))
                {
                    query = query.Where(s => s.Name.Contains(search));
                }

                if (!string.IsNullOrEmpty(sortBy))
                {
                    switch (sortBy.ToLower())
                    {
                        case "name":
                            query = query.OrderBy(s => s.Name);
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
            catch { throw; }
            var result = new PagedResult<SubjectViewModel>
            {
                Data = vmList,
                TotalItems = totalCount,
                PageNumber = pageNumber,
                PageSize = pageSize
            };
            return result;
        }

        public int GetAllSubjects()
        {
            var totalCount = _unitOfWork.GenericRepository<Subject>().GetAll().Count();
            return totalCount;
        }

        private List<SubjectViewModel> ConvertModelToViewModelList(List<Subject> modelList)
        {
            return modelList.Select(x => new SubjectViewModel(x)).ToList();
        }
    }
}