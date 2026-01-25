using Microsoft.AspNetCore.Mvc.RazorPages;
using SMS.Utilities;
using SMS.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMS.Services
{
    public interface IGradeService
    {
        Task Add(CreateGradeViewModel vm);
        int AddGradeWithStudent(GradeViewModel grade, int sessionId, List<int> StudentList);

        PagedResult <GradeViewModel> GetAll(int pageNumber, int pageSize);

        IEnumerable<GradeViewModel> GetAll();

        GradeViewModel GetById(int gradeId);
    }

}
