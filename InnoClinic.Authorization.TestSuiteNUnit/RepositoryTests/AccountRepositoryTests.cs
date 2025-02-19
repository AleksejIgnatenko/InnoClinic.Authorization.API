using InnoClinic.Authorization.Core.Enums;
using InnoClinic.Authorization.Core.Exceptions;
using InnoClinic.Authorization.Core.Models;
using InnoClinic.Authorization.DataAccess.Context;
using InnoClinic.Authorization.DataAccess.Repositories;
using Microsoft.EntityFrameworkCore;

namespace InnoClinic.Authorization.TestSuiteNUnit.RepositoryTests
{
    [TestFixture]
    public class AccountRepositoryTests
    {
        private AccountRepository _repository;
        private InnoClinicAuthorizationDbContext _context;

        private AccountModel _accountExample;

        [SetUp]
        public void Setup()
        {
            var options = new DbContextOptionsBuilder<InnoClinicAuthorizationDbContext>()
                .UseInMemoryDatabase(databaseName: "TestAuthDb")
                .Options;

            _context = new InnoClinicAuthorizationDbContext(options);
            _repository = new AccountRepository(_context);
            _accountExample = new AccountModel
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
        }

        [TearDown]
        public void TearDown()
        {
            _context.Database.EnsureDeleted();
            _context.Dispose();
        }

        [Test]
        public async Task CreateAsync_AddsAccountToDatabase()
        {
            // Act
            await _repository.CreateAsync(_accountExample);
            var result = await _context.Accounts.FindAsync(_accountExample.Id);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(_accountExample.Email, result.Email);
        }

        [Test]
        public async Task GetAllAsync_ReturnsAllAccounts()
        {
            // Arrange
            var account1 = new AccountModel
            {
                Id = Guid.NewGuid(),
                Email = "user1@example.com",
                Password = "password1",
                PhoneNumber = "PhoneNumber1",
                Role = Core.Enums.RoleEnum.Receptionist,
                RefreshToken = "RefreshToken1",
                RefreshTokenExpiryTime = DateTime.UtcNow,
                IsEmailVerified = true,
                PhotoId = Guid.NewGuid(),
                CreateBy = RoleEnum.Receptionist,
                CreateAt = DateTime.UtcNow,
                UpdateBy = RoleEnum.Receptionist,
                UpdateAt = DateTime.UtcNow,
            };
            var account2 = new AccountModel
            {
                Id = Guid.NewGuid(),
                Email = "user2@example.com",
                Password = "password2",
                PhoneNumber = "PhoneNumber2",
                Role = RoleEnum.Receptionist,
                RefreshToken = "RefreshToken2",
                RefreshTokenExpiryTime = DateTime.UtcNow,
                IsEmailVerified = true,
                PhotoId = Guid.NewGuid(),
                CreateBy = RoleEnum.Receptionist,
                CreateAt = DateTime.UtcNow,
                UpdateBy = RoleEnum.Receptionist,
                UpdateAt = DateTime.UtcNow,
            };

            // Act
            await _repository.CreateAsync(account1);
            await _repository.CreateAsync(account2);

            var result = await _repository.GetAllAsync();

            // Assert
            Assert.AreEqual(2, result.Count());
        }

        [Test]
        public async Task GetByIdAsync_ReturnsAccount_WhenExists()
        {
            // Act
            await _repository.CreateAsync(_accountExample);

            var result = await _repository.GetByIdAsync(_accountExample.Id);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(_accountExample.Email, result.Email);
        }

        [Test]
        public void GetByIdAsync_ThrowsException_WhenNotFound()
        {
            // Assert
            Assert.ThrowsAsync<DataRepositoryException>(async () => await _repository.GetByIdAsync(Guid.NewGuid()));
        }

        [Test]
        public async Task EmailExistsAsync_ReturnsTrue_WhenEmailExists()
        {
            // Act
            await _repository.CreateAsync(_accountExample);

            var result = await _repository.EmailExistsAsync(_accountExample.Email);

            // Assert
            Assert.IsTrue(result);
        }

        [Test]
        public async Task EmailExistsAsync_ReturnsFalse_WhenEmailDoesNotExist()
        {
            // Act
            var result = await _repository.EmailExistsAsync("nonexistent@example.com");

            // Assert
            Assert.IsFalse(result);
        }

        [Test]
        public async Task GetByEmailAsync_ReturnsAccount_WhenExists()
        {
            await _repository.CreateAsync(_accountExample);

            // Act
            var result = await _repository.GetByEmailAsync(_accountExample.Email);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(_accountExample.Email, result.Email);
        }

        [Test]
        public void GetByEmailAsync_ThrowsException_WhenNotFound()
        {
            // Assert
            Assert.ThrowsAsync<DataRepositoryException>(async () => await _repository.GetByEmailAsync("nonexistent@example.com"));
        }

        [Test]
        public async Task GetByIdsAsync_ReturnsCorrectAccounts()
        {
            // Arrange
            var account1 = new AccountModel
            {
                Id = Guid.NewGuid(),
                Email = "user1@example.com",
                Password = "password1",
                PhoneNumber = "PhoneNumber1",
                Role = Core.Enums.RoleEnum.Receptionist,
                RefreshToken = "RefreshToken1",
                RefreshTokenExpiryTime = DateTime.UtcNow,
                IsEmailVerified = true,
                PhotoId = Guid.NewGuid(),
                CreateBy = RoleEnum.Receptionist,
                CreateAt = DateTime.UtcNow,
                UpdateBy = RoleEnum.Receptionist,
                UpdateAt = DateTime.UtcNow,
            };
            var account2 = new AccountModel
            {
                Id = Guid.NewGuid(),
                Email = "user2@example.com",
                Password = "password2",
                PhoneNumber = "PhoneNumber2",
                Role = RoleEnum.Receptionist,
                RefreshToken = "RefreshToken2",
                RefreshTokenExpiryTime = DateTime.UtcNow,
                IsEmailVerified = true,
                PhotoId = Guid.NewGuid(),
                CreateBy = RoleEnum.Receptionist,
                CreateAt = DateTime.UtcNow,
                UpdateBy = RoleEnum.Receptionist,
                UpdateAt = DateTime.UtcNow,
            };

            // Act
            await _repository.CreateAsync(account1);
            await _repository.CreateAsync(account2);

            var accountIds = new List<Guid> { account1.Id, account2.Id };
            var expectedAccountsCount = 2;

            var accounts = await _repository.GetByIdAsync(accountIds);

            // Assert
            Assert.NotNull(accounts);
            Assert.AreEqual(expectedAccountsCount, accounts.Count);
        }

        [Test]
        public async Task UpdateAsync_UpdatesAccount()
        {
            // Act
            await _repository.CreateAsync(_accountExample);

            _accountExample.Email = "updated@example.com";
            await _repository.UpdateAsync(_accountExample);

            var result = await _repository.GetByIdAsync(_accountExample.Id);

            // Assert
            Assert.AreEqual("updated@example.com", result.Email);
        }

        [Test]
        public async Task UpdatePhoneNumberAsync_UpdatesAccount()
        {
            // Act
            await _repository.CreateAsync(_accountExample);

            await _repository.UpdateAsync(_accountExample.Id, "new phoneNumber");

            var result = await _repository.GetByIdAsync(_accountExample.Id);

            // Assert
            Assert.AreEqual("new phoneNumber", result.PhoneNumber);
        }

        [Test]
        public async Task DeleteAsync_DeletesEntity()
        {
            // Act
            await _repository.CreateAsync(_accountExample);

            await _repository.DeleteAsync(_accountExample);

            // Assert
            Assert.ThrowsAsync<DataRepositoryException>(async () => await _repository.GetByIdAsync(_accountExample.Id));
        }

        [Test]
        public async Task GetByRefreshTokenAsync_ExistingRefreshToken_ReturnsAccount()
        {
            await _repository.CreateAsync(_accountExample);

            // Act
            var result = await _repository.GetByRefreshTokenAsync(_accountExample.RefreshToken);

            // Assert
            Assert.AreEqual(result.Id, _accountExample.Id);
        }

        [Test]
        public void GetByRefreshTokenAsync_NonExistingRefreshToken_ThrowsDataRepositoryException()
        {
            // Arrange
            var refreshToken = "nonExistingRefreshToken";

            // Act & Assert
            var exception = Assert.ThrowsAsync<DataRepositoryException>(
                async () => await _repository.GetByRefreshTokenAsync(refreshToken));

            // Assert
            Assert.AreEqual($"Account with refresh token '{refreshToken}' not found.", exception.Message);
        }
    }
}
