using Moq;
using DoctorsClinic.Core.Services;
using DoctorsClinic.Infrastructure.IRepositories;
using DoctorsClinic.Core.IServices.Account;
using DoctorsClinic.Domain.Entities;

public class DoctorServiceTests
{
    [Fact]
    public async Task GetById_ShouldReturnDoctorResponseDto_WhenDoctorExists()
    {
        // Arrange: تجهيز بيانات الطبيب الوهمية
        var doctor = new Doctor
        {
            Id = 1,
            FullName = "Dr. Abbas al-Numani",
            Specialty = new Specialty { Id = 1, Name = "Cardiology" },
            Appointments = new List<Appointment>(),
            MedicalRecords = new List<MedicalRecord>(),
            Prescriptions = new List<Prescription>()
        };

        // تجهيز Mock للـ DoctorRepo بحيث FindByCondition يرجع الطبيب إذا id=1
        var mockDoctorRepo = new Mock<IDoctorRepo>();
        mockDoctorRepo.Setup(r =>
            r.FindByCondition(It.IsAny<System.Linq.Expressions.Expression<System.Func<Doctor, bool>>>(), true, true))
            .Returns((System.Linq.Expressions.Expression<System.Func<Doctor, bool>> predicate, bool track, bool byTenant) =>
                new List<Doctor> { doctor }.AsQueryable());

        // تجهيز Mock لـ IRepositoryWrapper بحيث DoctorRepo يرجع الـ MockDoctorRepo.Object
        var mockWrapper = new Mock<IRepositoryWrapper>();
        mockWrapper.SetupGet(w => w.DoctorRepo).Returns(mockDoctorRepo.Object);

        // Mock لـ IUserAccessorService (حتى لو لم نستخدمه بالاختبار)
        var mockUserAccessor = new Mock<IUserAccessorService>();

        var service = new DoctorService(mockWrapper.Object, mockUserAccessor.Object);

        // Act: استدعاء الدالة
        var result = await service.GetById(1);

        // Assert: التحقق من صحة النتيجة
        Assert.NotNull(result);
        Assert.False(result.Error, "Should not have error when doctor exists");
        Assert.NotNull(result.Data);
        Assert.Equal("Dr. Abbas al-Numani", result.Data.Doctor.FullName);
        Assert.Equal(1, result.Data.Doctor.Id);
    }
}
