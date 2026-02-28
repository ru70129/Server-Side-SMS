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
    public class SessionService : ISessionService
    {
        private IUnitOfWork _unitOfWork;
        public SessionService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task Update(SessionViewModel vm)
        {
            var session = _unitOfWork.GenericRepository<Session>().GetById(vm.Id);
            if (session == null) return;

            var oldValues = System.Text.Json.JsonSerializer.Serialize(session);

            session.Start = vm.Start;
            session.End = vm.End;
            // no UpdatedBy on session model currently; if needed add fields

            _unitOfWork.GenericRepository<Session>().Update(session);
            await _unitOfWork.SaveAsync();

            var audit = new AuditLog
            {
                Action = "UPDATE",
                EntityName = "Session",
                EntityId = session.Id,
                OldValues = oldValues,
                NewValues = System.Text.Json.JsonSerializer.Serialize(session),
                UserId = vm.UpdatedBy
            };
            _unitOfWork.GenericRepository<AuditLog>().Add(audit);
            await _unitOfWork.SaveAsync();
        }

        public async Task Delete(int id, string deletedBy = "System")
        {
            var session = _unitOfWork.GenericRepository<Session>().GetById(id);
            if (session == null) return;

            var oldValues = System.Text.Json.JsonSerializer.Serialize(session);

            _unitOfWork.GenericRepository<Session>().Delete(session);
            await _unitOfWork.SaveAsync();

            var audit = new AuditLog
            {
                Action = "DELETE",
                EntityName = "Session",
                EntityId = id,
                OldValues = oldValues,
                UserId = deletedBy
            };
            _unitOfWork.GenericRepository<AuditLog>().Add(audit);
            await _unitOfWork.SaveAsync();
        }
        public async Task Add(CreateSessionViewModel vm)
        {
            var model = new CreateSessionViewModel().Convert(vm);
            await _unitOfWork.GenericRepository<Session>().AddAsync(model);
            await _unitOfWork.SaveAsync();

            var audit = new AuditLog
            {
                Action = "CREATE",
                EntityName = "Session",
                EntityId = model.Id,
                NewValues = System.Text.Json.JsonSerializer.Serialize(model),
                UserId = vm.CreatedBy
            };
            _unitOfWork.GenericRepository<AuditLog>().Add(audit);
            await _unitOfWork.SaveAsync();
        }
        public PagedResult<SessionViewModel> GetAll(int pageNumber, int pageSize)
        {
            int totalCount = 0;
            List<SessionViewModel> vmList = new List<SessionViewModel>();
            try
            {
                int ExcludeRecords = (pageSize * pageNumber) - pageSize;
                var modelList = _unitOfWork.GenericRepository<Session>().GetAll()
                    .Skip(ExcludeRecords).Take(pageSize).ToList();
                totalCount = _unitOfWork.GenericRepository<Session>().GetAll().ToList().Count;
                vmList = ConvertModelToViewModelList(modelList);
            }
            catch (Exception ex) { throw; }
            var result = new PagedResult<SessionViewModel>
            {
                Data = vmList,
                TotalItems = totalCount,
                PageNumber = pageNumber,
                PageSize = pageSize
            };
            return result;
        }

        public IEnumerable<SessionViewModel> GetAll()
        {
            var modelList = _unitOfWork.GenericRepository<Session>().GetAll();
            var vmList = ConvertModelToViewModelList(modelList.ToList());
            return vmList;
        }

        public SessionViewModel GetById(int sessionId)
        {
            return new SessionViewModel();
            //var model = _unitOfWork.GenericRepository<Session>().GetById();
            //var vm = new SessionViewModel(model);
        }

        private List<SessionViewModel> ConvertModelToViewModelList(List<Session>modelList)
        {
            return modelList.Select(x => new SessionViewModel(x)).ToList();
        }
    }
}
