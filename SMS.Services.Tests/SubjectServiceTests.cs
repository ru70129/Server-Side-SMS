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
    public class SubjectServiceTests
    {
        private Mock<IUnitOfWork> _unitOfWorkMock;
        private Mock<UserManager<ApplicationUser>> _userManagerMock;
        private Mock<RoleManager<IdentityRole>> _roleManagerMock;
        private SubjectService _subjectService;

        public SubjectServiceTests()
        {
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _userManagerMock = new Mock<UserManager<ApplicationUser>>();
            _roleManagerMock = new Mock<RoleManager<IdentityRole>>();
            _subjectService = new SubjectService(_unitOfWorkMock.Object, _userManagerMock.Object, _roleManagerMock.Object);
        }

        [Fact]
        public async Task AddSubject_ShouldAddSubject()
        {
            // Arrange
            var vm = new CreateSubjectViewModel { Name = "Math" };
            _unitOfWorkMock.Setup(u => u.GenericRepository<Subject>().Add(It.IsAny<Subject>()));
            _unitOfWorkMock.Setup(u => u.SaveAsync()).Returns(Task.CompletedTask);

            // Act
            await _subjectService.AddSubject(vm);

            // Assert
            _unitOfWorkMock.Verify(u => u.GenericRepository<Subject>().Add(It.IsAny<Subject>()), Times.Once);
            _unitOfWorkMock.Verify(u => u.SaveAsync(), Times.Once);
        }

        [Fact]
        public void GetAll_ShouldReturnPagedResult()
        {
            // Arrange
            var subjects = new List<Subject> { new Subject { Id = 1, Name = "Math" } };
            _unitOfWorkMock.Setup(u => u.GenericRepository<Subject>().GetAll()).Returns(subjects.AsQueryable());

            // Act
            var result = _subjectService.GetAll(1, 10);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(1, result.Data.Count);
        }
    }
}