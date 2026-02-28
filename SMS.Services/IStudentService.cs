using SMS.Utilities;
using SMS.ViewModels;
using System.Threading.Tasks;

namespace SMS.Services
{
    public interface IStudentService
    {
        Task AddStudent(CreateStudentViewModel student);
        Task UpdateStudent(StudentViewModel student);
        Task DeleteStudent(int id);
        StudentViewModel? GetById(int id);
        PagedResult<StudentViewModel> GetAll(int pageNumber, int pageSize, string? search = null, string? sortBy = null, bool isActive = true);
        int GetAllStudents();
    }
}
