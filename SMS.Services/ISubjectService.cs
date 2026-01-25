using SMS.Utilities;
using SMS.ViewModels;
using System.Threading.Tasks;

namespace SMS.Services
{
    public interface ISubjectService
    {
        Task AddSubject(CreateSubjectViewModel subject);
        Task UpdateSubject(SubjectViewModel subject);
        Task DeleteSubject(int id);
        SubjectViewModel GetById(int id);
        PagedResult<SubjectViewModel> GetAll(int pageNumber, int pageSize, string search = null, string sortBy = null, bool isActive = true);
        int GetAllSubjects();
    }
}