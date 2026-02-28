using System.Threading.Tasks;
using Moq;
using SMS.Models;
using SMS.Repositories;
using SMS.Services;
using SMS.ViewModels;
using Xunit;
using Microsoft.AspNetCore.Identity;

namespace SMS.Services.Tests
{
    public class AuditTests
    {
        [Fact]
        public async Task StudentService_Add_CreatesAudit()
        {
            var unitMock = new Mock<IUnitOfWork>();
            var studentRepo = new Mock<IGenericRepository<Student>>();
            var auditRepo = new Mock<IGenericRepository<AuditLog>>();

            unitMock.Setup(u => u.GenericRepository<Student>()).Returns(studentRepo.Object);
            unitMock.Setup(u => u.GenericRepository<AuditLog>()).Returns(auditRepo.Object);
            unitMock.Setup(u => u.SaveAsync()).Returns(Task.CompletedTask);

            var userManager = MockUserManager();
            var roleManager = MockRoleManager();

            var httpMock = new Mock<Microsoft.AspNetCore.Http.IHttpContextAccessor>();
            var svc = new StudentService(unitMock.Object, userManager.Object, roleManager.Object, httpMock.Object);
            var vm = new CreateStudentViewModel { FirstName = "John", LastName = "Doe", CreatedBy = "tester" };
            await svc.AddStudent(vm);

            auditRepo.Verify(r => r.Add(It.IsAny<AuditLog>()), Times.Once);
        }

        [Fact]
        public async Task TeacherService_Update_CreatesAudit()
        {
            var unitMock = new Mock<IUnitOfWork>();
            var teacherRepo = new Mock<IGenericRepository<Teacher>>();
            var auditRepo = new Mock<IGenericRepository<AuditLog>>();

            var existing = new Teacher { Id = 1, FirstName = "A", LastName = "B", CreatedBy = "tester" };
            teacherRepo.Setup(r => r.GetById(It.IsAny<object>())).Returns(existing);

            unitMock.Setup(u => u.GenericRepository<Teacher>()).Returns(teacherRepo.Object);
            unitMock.Setup(u => u.GenericRepository<AuditLog>()).Returns(auditRepo.Object);
            unitMock.Setup(u => u.SaveAsync()).Returns(Task.CompletedTask);

            var userManager = MockUserManager();
            var roleManager = MockRoleManager();

            var httpMock2 = new Mock<Microsoft.AspNetCore.Http.IHttpContextAccessor>();
            var svc = new TeacherService(unitMock.Object, userManager.Object, roleManager.Object, httpMock2.Object);
            var vm = new TeacherViewModel(existing) { UpdatedBy = "tester" };
            await svc.UpdateTeacher(vm);

            auditRepo.Verify(r => r.Add(It.IsAny<AuditLog>()), Times.Once);
        }

        [Fact]
        public async Task GradeService_Delete_CreatesAudit()
        {
            var unitMock = new Mock<IUnitOfWork>();
            var gradeRepo = new Mock<IGenericRepository<Grade>>();
            var auditRepo = new Mock<IGenericRepository<AuditLog>>();

            var existing = new Grade { Id = 1, Name = "G1", CreatedBy = "tester" };
            gradeRepo.Setup(r => r.GetById(It.IsAny<object>())).Returns(existing);

            unitMock.Setup(u => u.GenericRepository<Grade>()).Returns(gradeRepo.Object);
            unitMock.Setup(u => u.GenericRepository<AuditLog>()).Returns(auditRepo.Object);
            unitMock.Setup(u => u.SaveAsync()).Returns(Task.CompletedTask);

            var svc = new GradeService(unitMock.Object);
            await svc.Delete(1, "tester");

            auditRepo.Verify(r => r.Add(It.IsAny<AuditLog>()), Times.Once);
        }

        private Mock<UserManager<ApplicationUser>> MockUserManager()
        {
            var store = new Mock<IUserStore<ApplicationUser>>();
            return new Mock<UserManager<ApplicationUser>>(store.Object, null, null, null, null, null, null, null, null);
        }

        private Mock<RoleManager<IdentityRole>> MockRoleManager()
        {
            var store = new Mock<IRoleStore<IdentityRole>>();
            return new Mock<RoleManager<IdentityRole>>(store.Object, null, null, null, null);
        }
    }
}
