using InnoClinic.Authorization.Application.Services;
using InnoClinic.Authorization.Core.Enums;
using InnoClinic.Authorization.Core.Models.AccountModels;

namespace InnoClinic.Authorization.TestSuiteNUnit.ServiceTests
{
    [TestFixture]
    public class ValidationServiceTests
    {
        private ValidationService _service;

        private AccountEntity accountExample = new AccountEntity
        {
            Id = Guid.NewGuid(),
            Email = "test@example.com",
            Password = "password",
            PhoneNumber = "PhoneNumber",
            Role = RoleEnum.Receptionist,
            RefreshToken = "RefreshToken",
            RefreshTokenExpiryTime = DateTime.UtcNow,
            IsEmailVerified = true,
            PhotoId = "PhotoId",
            CreateBy = RoleEnum.Receptionist.ToString(),
            CreateAt = DateTime.UtcNow,
            UpdateBy = RoleEnum.Receptionist.ToString(),
            UpdateAt = DateTime.UtcNow,
        };

        [SetUp]
        public void Setup()
        {
            _service = new ValidationService(); 
        }

        [Test]
        public void ValidateAccountModel_WhenValidData()
        {
            // Arrange
            var invalidAccount = accountExample;

            // Act
            var errors = _service.Validation(invalidAccount);

            // Assert
            Assert.AreEqual(0, errors.Count);
        }

        [Test]
        public void Validation_ReturnsErrors_WhenInvalidData()
        {
            // Arrange
            var invalidAccount = new AccountEntity
            {
                Id = Guid.NewGuid(),
                Email = "",
                Password = "",
                PhoneNumber = "PhoneNumber",
                Role = RoleEnum.Receptionist,
                RefreshToken = "RefreshToken",
                RefreshTokenExpiryTime = DateTime.UtcNow,
                IsEmailVerified = true,
                PhotoId = "PhotoId",
                CreateBy = RoleEnum.Receptionist.ToString(),
                CreateAt = DateTime.UtcNow,
                UpdateBy = RoleEnum.Receptionist.ToString(),
                UpdateAt = DateTime.UtcNow,
            };

            // Act
            var errors = _service.Validation(invalidAccount);

            // Assert
            Assert.AreEqual(2, errors.Count); 

            Assert.IsTrue(errors.ContainsKey("Email")); 
            Assert.IsTrue(errors.ContainsValue("Вы ввели неверный email"));

            Assert.IsTrue(errors.ContainsKey("Password"));
            Assert.IsTrue(errors.ContainsValue("Пожалуйста, введите пароль"));
        }
    }
}
