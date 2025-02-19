﻿using InnoClinic.Authorization.Application.Services;
using InnoClinic.Authorization.Core.Enums;
using InnoClinic.Authorization.Core.Models;

namespace InnoClinic.Authorization.TestSuiteNUnit.ServiceTests
{
    [TestFixture]
    public class ValidationServiceTests
    {
        private ValidationService _service;

        private AccountModel accountExample = new AccountModel
        {
            Id = Guid.NewGuid(),
            Email = "test@example.com",
            Password = "password",
            PhoneNumber = "PhoneNumber",
            Role = RoleEnum.Receptionist,
            RefreshToken = "RefreshToken",
            RefreshTokenExpiryTime = DateTime.UtcNow,
            IsEmailVerified = true,
            PhotoId = Guid.NewGuid(),
            CreateBy = RoleEnum.Receptionist,
            CreateAt = DateTime.UtcNow,
            UpdateBy = RoleEnum.Receptionist,
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
            var invalidAccount = new AccountModel
            {
                Id = Guid.NewGuid(),
                Email = "",
                Password = "",
                PhoneNumber = "PhoneNumber",
                Role = RoleEnum.Receptionist,
                RefreshToken = "RefreshToken",
                RefreshTokenExpiryTime = DateTime.UtcNow,
                IsEmailVerified = true,
                PhotoId = Guid.NewGuid(),
                CreateBy = RoleEnum.Receptionist,
                CreateAt = DateTime.UtcNow,
                UpdateBy = RoleEnum.Receptionist,
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
