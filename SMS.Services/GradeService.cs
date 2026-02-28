using Microsoft.EntityFrameworkCore;
using SMS.Models;
using SMS.Repositories;
using SMS.Utilities;
using SMS.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMS.Services
{
    public class GradeService : IGradeService
    {
        private IUnitOfWork _unitOfWork;

        public GradeService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task Add(CreateGradeViewModel vm)
        {
            var model = new CreateGradeViewModel().Convert(vm);
            await _unitOfWork.GenericRepository<Grade>().AddAsync(model);
            await _unitOfWork.SaveAsync();

            // Audit log
            var audit = new AuditLog
            {
                Action = "CREATE",
                EntityName = "Grade",
                EntityId = model.Id,
                NewValues = System.Text.Json.JsonSerializer.Serialize(model),
                UserId = vm.CreatedBy
            };
            _unitOfWork.GenericRepository<AuditLog>().Add(audit);
            await _unitOfWork.SaveAsync();
        }

        public int AddGradeWithStudent(GradeViewModel grade, int sessionId, List<int> StudentList)
        {
            int count = 0;
            var model = new GradeViewModel().Convert(grade);
            //List<Enroll> enrolls = new List<Enroll>();
            foreach (var item in StudentList)
            {
                if (!_unitOfWork.GenericRepository<Enroll>()
                    .Exists(x => x.StudentId == sessionId && x.StudentId == item))
                { 
                    model.Enrolls.Add(new Enroll()
                    {
                        StudentId = item,
                        GradeId = grade.Id,
                        SessionId = sessionId
                    });
                    count++;
                }
             }
            return count;
        }

        public async Task Update(GradeViewModel vm)
        {
            var grade = _unitOfWork.GenericRepository<Grade>().GetById(vm.Id);
            if (grade == null) return;

            var oldValues = System.Text.Json.JsonSerializer.Serialize(grade);

            grade.Name = vm.Name;
            grade.UpdatedBy = vm.UpdatedBy;
            grade.UpdatedAt = DateTime.Now;

            _unitOfWork.GenericRepository<Grade>().Update(grade);
            await _unitOfWork.SaveAsync();

            var audit = new AuditLog
            {
                Action = "UPDATE",
                EntityName = "Grade",
                EntityId = grade.Id,
                OldValues = oldValues,
                NewValues = System.Text.Json.JsonSerializer.Serialize(grade),
                UserId = vm.UpdatedBy
            };
            _unitOfWork.GenericRepository<AuditLog>().Add(audit);
            await _unitOfWork.SaveAsync();
        }

        public async Task Delete(int id, string deletedBy = "System")
        {
            var grade = _unitOfWork.GenericRepository<Grade>().GetById(id);
            if (grade == null) return;

            var oldValues = System.Text.Json.JsonSerializer.Serialize(grade);

            _unitOfWork.GenericRepository<Grade>().Delete(grade);
            await _unitOfWork.SaveAsync();

            var audit = new AuditLog
            {
                Action = "DELETE",
                EntityName = "Grade",
                EntityId = id,
                OldValues = oldValues,
                UserId = deletedBy
            };
            _unitOfWork.GenericRepository<AuditLog>().Add(audit);
            await _unitOfWork.SaveAsync();
        }

        public PagedResult<GradeViewModel> GetAll(int pageNumber, int pageSize)
        {
            int totalCount = 0;
            List<GradeViewModel> vmList = new List<GradeViewModel>();
            try
            {
                int ExcludeRecords = (pageSize * pageNumber) - pageSize;
                var modelList = _unitOfWork.GenericRepository<Grade>().GetAll()
                    .Skip(ExcludeRecords).Take(pageSize).ToList();
                totalCount = _unitOfWork.GenericRepository<Grade>().GetAll().ToList().Count;
                vmList = ConvertModelToViewModelList(modelList);
            }
            catch (Exception ex) { throw; }
            var result = new PagedResult<GradeViewModel>
            {
                Data = vmList,
                TotalItems = totalCount,
                PageNumber = pageNumber,
                PageSize = pageSize
            };
            return result;
        }

        public IEnumerable<GradeViewModel> GetAll()
        {
            var modelList = _unitOfWork.GenericRepository<Grade>().GetAll();
            var vmList = ConvertModelToViewModelList(modelList.ToList());
            return vmList;
        }
        public GradeViewModel GetById(int gradeId)
        {
            var model = _unitOfWork.GenericRepository<Grade>().GetByIdAsync(x => x.Id == gradeId, include: y => y.Include(a => a.Enrolls));
            var vm = new GradeViewModel(model);
            return vm;
        }
        private List<GradeViewModel> ConvertModelToViewModelList(List<Grade> modelList)
        {
            return modelList.Select(x => new GradeViewModel(x)).ToList();
        }
    }
}
