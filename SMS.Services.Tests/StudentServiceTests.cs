using SMS.Services;
using SMS.Repositories;
using SMS.Utilities;
using SMS.ViewModels;
using Microsoft.AspNetCore.Identity;
using Moq;
using System.Threading.Tasks;
using Xunit;

namespace SMS.Services.Tests
{
    public class StudentServiceTests
    {
        private Mock<IUnitOfWork> _unitOfWorkMock;
        private Mock<UserManager<ApplicationUser>> _userManagerMock;
        private Mock<RoleManager<IdentityRole>> _roleManagerMock;
        private Mock<Microsoft.AspNetCore.Http.IHttpContextAccessor> _httpContextAccessorMock;
        private StudentService _studentService;

        public StudentServiceTests()
        {
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _userManagerMock = new Mock<UserManager<ApplicationUser>>();
            _roleManagerMock = new Mock<RoleManager<IdentityRole>>();
            _httpContextAccessorMock = new Mock<Microsoft.AspNetCore.Http.IHttpContextAccessor>();
            _studentService = new StudentService(_unitOfWorkMock.Object, _userManagerMock.Object, _roleManagerMock.Object, _httpContextAccessorMock.Object);
        }

        [Fact]
        public async Task AddStudent_ShouldAddStudent()
        {
            // Arrange
            var vm = new CreateStudentViewModel { FirstName = "John", LastName = "Doe" };
            _unitOfWorkMock.Setup(u => u.GenericRepository<Student>().Add(It.IsAny<Student>()));
            _unitOfWorkMock.Setup(u => u.SaveAsync()).Returns(Task.CompletedTask);

            // Act
            await _studentService.AddStudent(vm);

            // Assert
            _unitOfWorkMock.Verify(u => u.GenericRepository<Student>().Add(It.IsAny<Student>()), Times.Once);
            _unitOfWorkMock.Verify(u => u.SaveAsync(), Times.Once);
        }

        [Fact]
        public void GetAll_ShouldReturnPagedResult()
        {
            // Arrange
            var students = new List<Student> { new Student { Id = 1, FirstName = "John" } };
            _unitOfWorkMock.Setup(u => u.GenericRepository<Student>().GetAll()).Returns(students.AsQueryable());

            // Act
            var result = _studentService.GetAll(1, 10);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(1, result.Data.Count);
        }
    }
}