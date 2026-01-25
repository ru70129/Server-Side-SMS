using SMS.Utilities;
using SMS.ViewModels;
using System.Threading.Tasks;

namespace SMS.Services
{
    public interface ITeacherService
    {
        Task AddTeacher(CreateTeacherViewModel teacher);
        Task UpdateTeacher(TeacherViewModel teacher);
        Task DeleteTeacher(int id);
        TeacherViewModel GetById(int id);
        PagedResult<TeacherViewModel> GetAll(int pageNumber, int pageSize, string search = null, string sortBy = null, bool isActive = true);
        int GetAllTeachers();
    }
}