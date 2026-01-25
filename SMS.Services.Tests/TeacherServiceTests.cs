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
    public class TeacherServiceTests
    {
        private Mock<IUnitOfWork> _unitOfWorkMock;
        private Mock<UserManager<ApplicationUser>> _userManagerMock;
        private Mock<RoleManager<IdentityRole>> _roleManagerMock;
        private TeacherService _teacherService;

        public TeacherServiceTests()
        {
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _userManagerMock = new Mock<UserManager<ApplicationUser>>();
            _roleManagerMock = new Mock<RoleManager<IdentityRole>>();
            _teacherService = new TeacherService(_unitOfWorkMock.Object, _userManagerMock.Object, _roleManagerMock.Object);
        }

        [Fact]
        public async Task AddTeacher_ShouldAddTeacher()
        {
            // Arrange
            var vm = new CreateTeacherViewModel { FirstName = "Jane", LastName = "Smith" };
            _unitOfWorkMock.Setup(u => u.GenericRepository<Teacher>().Add(It.IsAny<Teacher>()));
            _unitOfWorkMock.Setup(u => u.SaveAsync()).Returns(Task.CompletedTask);

            // Act
            await _teacherService.AddTeacher(vm);

            // Assert
            _unitOfWorkMock.Verify(u => u.GenericRepository<Teacher>().Add(It.IsAny<Teacher>()), Times.Once);
            _unitOfWorkMock.Verify(u => u.SaveAsync(), Times.Once);
        }

        [Fact]
        public void GetAll_ShouldReturnPagedResult()
        {
            // Arrange
            var teachers = new List<Teacher> { new Teacher { Id = 1, FirstName = "Jane" } };
            _unitOfWorkMock.Setup(u => u.GenericRepository<Teacher>().GetAll()).Returns(teachers.AsQueryable());

            // Act
            var result = _teacherService.GetAll(1, 10);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(1, result.Data.Count);
        }
    }
}