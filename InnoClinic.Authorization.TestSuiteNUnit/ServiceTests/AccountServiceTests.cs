using AutoMapper;
using InnoClinic.Authorization.Application.Services;
using InnoClinic.Authorization.Core.Enums;
using InnoClinic.Authorization.Core.Exceptions;
using InnoClinic.Authorization.Core.Models.AccountModels;
using InnoClinic.Authorization.DataAccess.Repositories;
using InnoClinic.Authorization.Infrastructure.Jwt;
using InnoClinic.Authorization.Infrastructure.RabbitMQ;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Moq;
using System.Security.Claims;

namespace InnoClinic.Authorization.TestSuiteNUnit.ServiceTests
{
    public class AccountServiceTests
    {
        private AccountService _accountService;
        private Mock<IAccountRepository> _accountRepositoryMock;
        private Mock<IJwtTokenService> _jwtTokenServiceMock;
        private Mock<IValidationService> _validationServiceMock;
        private Mock<IEmailService> _emailVerificationServiceMock;
        private Mock<IRabbitMQService> _rabbitmqServiceMock;
        private Mock<IDoctorRepository> _doctorRepositoryMock;
        private IMapper _mapper;
        private List<Claim> _claims;

        [SetUp]
        public void Setup()
        {
            _accountRepositoryMock = new Mock<IAccountRepository>();
            _jwtTokenServiceMock = new Mock<IJwtTokenService>();
            _validationServiceMock = new Mock<IValidationService>();
            _emailVerificationServiceMock = new Mock<IEmailService>();
            _rabbitmqServiceMock = new Mock<IRabbitMQService>();
            _doctorRepositoryMock = new Mock<IDoctorRepository>();

            var jwtOptions = Options.Create(new JwtOptions
            {
                SecretKey = "secretkeysecretkeysecretkeysecretkeysecretkeysecretkeysecretkey",
                Issuer = "http://localhost:5001",
                Audience = new List<string>
                {
                    "http://localhost:5001",
                    "http://localhost:5002",
                    "http://localhost:5003",
                    "http://localhost:5004",
                    "http://localhost:5005"
                },
                AccessTokenExpirationMinutes = 15,
                RefreshTokenExpirationDays = 180,

            });

            _claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, Guid.NewGuid().ToString()),
                new Claim(ClaimTypes.Role, RoleEnum.Receptionist.ToString()),
            };

            var config = new MapperConfiguration(cfg => cfg.CreateMap<AccountEntity, AccountDto>());
            _mapper = config.CreateMapper();

            _accountService = new AccountService(
                _accountRepositoryMock.Object,
                _jwtTokenServiceMock.Object,
                jwtOptions,
                _validationServiceMock.Object,
                _emailVerificationServiceMock.Object,
                _mapper,
                _rabbitmqServiceMock.Object,
                _doctorRepositoryMock.Object);
        }

        [Test]
        public async Task CreateAccountAsync_ValidAccount_CreatesAccountAndSendsEmail()
        {
            // Arrange
            var email = "test@example.com";
            var password = "StrongPassword123!";
            var urlHelper = new Mock<IUrlHelper>();

            _validationServiceMock.Setup(v => v.Validation(It.IsAny<AccountEntity>())).Returns(new Dictionary<string, string>());
            _jwtTokenServiceMock.Setup(j => j.GenerateAccessToken(It.IsAny<IEnumerable<Claim>>())).Returns("accessToken");
            _jwtTokenServiceMock.Setup(j => j.GenerateRefreshToken()).Returns("refreshToken");

            // Act
            var result = await _accountService.CreateAccountAsync(email, password, urlHelper.Object);

            // Assert
            Assert.AreEqual("accessToken", result.accessToken);
            Assert.AreEqual("refreshToken", result.refreshToken);
            Assert.AreEqual("Для подтверждения почты проверьте электронную почту и перейдите по ссылке, указанной в письме.", result.message);
            _accountRepositoryMock.Verify(a => a.CreateAsync(It.IsAny<AccountEntity>()), Times.Once);
            _emailVerificationServiceMock.Verify(e => e.SendVerificationEmailAsync(It.IsAny<AccountEntity>(), urlHelper.Object), Times.Once);
            _rabbitmqServiceMock.Verify(r => r.PublishMessageAsync(It.IsAny<AccountDto>(), RabbitMQQueues.ADD_ACCOUNT_QUEUE), Times.Once);
        }

        [Test]
        public async Task CreateAccountAsync_InvalidAccount_ThrowsValidationException()
        {
            // Arrange
            var email = "";
            var password = "";
            var urlHelper = new Mock<IUrlHelper>();

            _validationServiceMock.Setup(v => v.Validation(It.IsAny<AccountEntity>())).Returns(new Dictionary<string, string> { { "Email", "Вы ввели неверный email" }, { "Password", "Пожалуйста, введите пароль" } });

            // Act & Assert
            var errors = Assert.ThrowsAsync<ValidationException>(
                async () => await _accountService.CreateAccountAsync(email, password, urlHelper.Object));
        }

        [Test]
        public async Task LoginAsync_ValidAccount_ReturnsHashedPasswordAccessTokenRefreshToken()
        {
            // Arrange
            var email = "test@example.com";
            var account = new AccountEntity
            {
                Email = email,
                Password = "StrongPassword123!",
            };

            _accountRepositoryMock.Setup(r => r.GetByEmailAsync(email)).ReturnsAsync(account);
            _jwtTokenServiceMock.Setup(j => j.GenerateAccessToken(_claims)).Returns("accessToken");
            _jwtTokenServiceMock.Setup(j => j.GenerateRefreshToken()).Returns("refreshToken");

            // Act
            var result = await _accountService.LoginAsync(email);

            // Assert
            Assert.IsNotEmpty(result.hashPassword);
            Assert.IsNotEmpty(result.accessToken);
            Assert.IsNotEmpty(result.refreshToken);
            _accountRepositoryMock.Verify(r => r.UpdateAsync(account), Times.Once);
        }

        [Test]
        public void LoginAsync_InvalidAccount_ThrowsException()
        {
            // Arrange
            var email = "nonexistent@example.com";
            _accountRepositoryMock.Setup(r => r.GetByEmailAsync(email)).ThrowsAsync(new DataException($"Account with email '{email}' not found.", StatusCodes.Status404NotFound));

            // Act & Assert
            Assert.ThrowsAsync<DataException>(async () => await _accountService.LoginAsync(email));
        }

        [Test]
        public async Task RefreshTokenAsync_ExistingRefreshToken_GeneratesNewTokens()
        {
            // Arrange
            var refreshToken = "validRefreshToken";
            var account = new AccountEntity
            {
                RefreshToken = refreshToken,
                RefreshTokenExpiryTime = DateTime.UtcNow.AddHours(1)
            };

            _accountRepositoryMock.Setup(r => r.GetByRefreshTokenAsync(refreshToken)).ReturnsAsync(account);
            _jwtTokenServiceMock.Setup(j => j.GenerateAccessToken(_claims)).Returns("newAccessToken");
            _jwtTokenServiceMock.Setup(j => j.GenerateRefreshToken()).Returns("newRefreshToken");

            // Act
            var result = await _accountService.RefreshTokenAsync(refreshToken);

            // Assert
            Assert.IsNotEmpty(result.accessToken);
            Assert.IsNotEmpty(result.refreshToken);
            _accountRepositoryMock.Verify(r => r.UpdateAsync(account), Times.Once);
        }

        [Test]
        public void RefreshTokenAsync_NonExistingRefreshToken_ThrowsException()
        {
            // Arrange
            var refreshToken = "nonExistingRefreshToken";
            _accountRepositoryMock.Setup(r => r.GetByRefreshTokenAsync(refreshToken)).ThrowsAsync(new DataException($"Account with refresh token '{refreshToken}' not found.", StatusCodes.Status404NotFound));

            // Act & Assert
            Assert.ThrowsAsync<DataException>(async () => await _accountService.RefreshTokenAsync(refreshToken));
        }

        [Test]
        public void RefreshTokenAsync_ExpiredRefreshToken_ThrowsException()
        {
            // Arrange
            var refreshToken = "validRefreshToken";
            var account = new AccountEntity
            {
                RefreshToken = refreshToken,
                RefreshTokenExpiryTime = DateTime.UtcNow.AddHours(-1)
            };

            _accountRepositoryMock.Setup(r => r.GetByRefreshTokenAsync(refreshToken)).ReturnsAsync(account);

            // Act & Assert
            var exception = Assert.ThrowsAsync<Exception>(async () => await _accountService.RefreshTokenAsync(refreshToken));
            Assert.AreEqual("Invalid refresh token.", exception.Message);
        }

        [Test]
        public async Task ConfirmEmailAsync_ValidToken_ConfirmsEmailAndReturnsTrue()
        {
            // Arrange
            var accountId = Guid.NewGuid();
            var token = "validToken";
            var account = new AccountEntity
            {
                Id = accountId,
                IsEmailVerified = false
            };

            _accountRepositoryMock.Setup(r => r.GetByIdAsync(accountId)).ReturnsAsync(account);
            _emailVerificationServiceMock.Setup(e => e.ConfirmEmail(token)).Returns("email@example.com");

            // Act
            var result = await _accountService.ConfirmEmailAsync(accountId, token);

            // Assert
            Assert.IsTrue(result);
            Assert.IsTrue(account.IsEmailVerified);
            _accountRepositoryMock.Verify(r => r.UpdateAsync(account), Times.Once);
        }

        [Test]
        public async Task ConfirmEmailAsync_InvalidToken_DoesNotConfirmEmailAndReturnsFalse()
        {
            // Arrange
            var accountId = Guid.NewGuid();
            var token = "invalidToken";
            var account = new AccountEntity
            {
                Id = accountId,
                IsEmailVerified = false
            };

            _accountRepositoryMock.Setup(r => r.GetByIdAsync(accountId)).ReturnsAsync(account);
            _emailVerificationServiceMock.Setup(e => e.ConfirmEmail(token)).Returns("");

            // Act
            var result = await _accountService.ConfirmEmailAsync(accountId, token);

            // Assert
            Assert.IsFalse(result);
            Assert.IsFalse(account.IsEmailVerified); 
            _accountRepositoryMock.Verify(r => r.UpdateAsync(account), Times.Never);
        }

        [Test]
        public async Task EmailExistsAsync_EmailExists_ReturnsTrue()
        {
            // Arrange
            var email = "test@example.com";
            _accountRepositoryMock.Setup(r => r.EmailExistsAsync(email)).ReturnsAsync(true);

            // Act
            var result = await _accountService.EmailExistsAsync(email);

            // Assert
            Assert.IsTrue(result);
        }

        [Test]
        public async Task EmailExistsAsync_EmailDoesNotExist_ReturnsFalse()
        {
            // Arrange
            var email = "nonexistent@example.com";
            _accountRepositoryMock.Setup(r => r.EmailExistsAsync(email)).ReturnsAsync(false);

            // Act
            var result = await _accountService.EmailExistsAsync(email);

            // Assert
            Assert.IsFalse(result);
        }

        [Test]
        public async Task GetAllAccountsAsync_ReturnsAllAccounts()
        {
            // Arrange
            var mockAccounts = new List<AccountEntity>
            {
                new AccountEntity { Id = Guid.NewGuid(), Email = "test1@example.com" },
                new AccountEntity { Id = Guid.NewGuid(), Email = "test2@example.com" },
                new AccountEntity { Id = Guid.NewGuid(), Email = "test3@example.com" }
            };

            _accountRepositoryMock.Setup(r => r.GetAllAsync()).ReturnsAsync(mockAccounts);

            // Act
            var result = await _accountService.GetAllAccountsAsync();

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(mockAccounts.Count, result.Count());
            Assert.IsTrue(result.All(a => mockAccounts.Any(m => m.Id == a.Id && m.Email == a.Email)));
        }

        [Test]
        public async Task GetAccountByIdAsync_ReturnsAccountById()
        {
            // Arrange
            var accountId = Guid.NewGuid();
            var token = "validJwtToken";
            var account = new AccountEntity
            {
                Id = accountId,
                Email = "test@example.com"
            };

            _jwtTokenServiceMock.Setup(j => j.GetAccountIdFromAccessToken(token)).Returns(accountId);
            _accountRepositoryMock.Setup(r => r.GetByIdAsync(accountId)).ReturnsAsync(account);

            // Act
            var result = await _accountService.GetAccountByIdAsync(token);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(accountId, result.Id);
            Assert.AreEqual(account.Email, result.Email);
        }

        [Test]
        public async Task GetAccountsByIdsAsync_ReturnsAllAccounts()
        {
            // Arrange
            var mockAccounts = new List<AccountEntity>
            {
                new AccountEntity { Id = Guid.NewGuid(), Email = "test1@example.com" },
                new AccountEntity { Id = Guid.NewGuid(), Email = "test2@example.com" },
                new AccountEntity { Id = Guid.NewGuid(), Email = "test3@example.com" }
            };

            var ids = new List<Guid>
            {
                mockAccounts[0].Id, 
                mockAccounts[1].Id, 
                mockAccounts[2].Id
            };

            _accountRepositoryMock.Setup(r => r.GetByIdAsync(ids)).ReturnsAsync(mockAccounts);

            // Act
            var result = await _accountService.GetAccountsByIdsAsync(ids);

            // Assert
            Assert.AreEqual(mockAccounts.Count, result.Count());
        }
    }
}
