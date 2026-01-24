using SMS.Utilities;
using SMS.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMS.Services
{
    public interface IStudentService
    {
        Task AddStudent(CreateStudentViewModel student);

        PagedResult<StudentViewModel> GetAll(int pageNumber, int pageSize);

        int GetAllStudents();

    }
}
