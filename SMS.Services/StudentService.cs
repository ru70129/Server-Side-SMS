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

namespace SMS.Services
{
    public class StudentService : IStudentService
    {
        private IUnitOfWork _unitOfWork;
        private UserManager<ApplicationUser> _userManager;
        private RoleManager<IdentityRole> _roleManager;

        public StudentService(IUnitOfWork unitOfWork,
            UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager)
        {
            _unitOfWork = unitOfWork;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public async Task AddStudent(CreateStudentViewModel student)
        {
        }

        public PagedResult<StudentViewModel> GetAll(int pageNumber, int pageSize)
        {
            int totalCount = 0;
            List<StudentViewModel> vmList = new List<StudentViewModel>();
            try
            {
                int ExcludeRecords = (pageSize * pageNumber) - pageSize;
                var modelList = _unitOfWork.GenericRepository<Student>().GetAll()
                    .Skip(ExcludeRecords).Take(pageSize).ToList();
                totalCount = _unitOfWork.GenericRepository<Student>().GetAll().ToList().Count;
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
            var totalCount = _unitOfWork.GenericRepository<Student>().GetAll().ToList().Count;
            return totalCount;
        }

        private List<StudentViewModel> ConvertModelToViewModelList(List<Student> modelList)
        {
            return modelList.Select(x => new StudentViewModel(x)).ToList();
        }



    }
}
