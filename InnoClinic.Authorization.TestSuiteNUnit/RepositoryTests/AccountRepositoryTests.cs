using InnoClinic.Authorization.Core.Enums;
using InnoClinic.Authorization.Core.Exceptions;
using InnoClinic.Authorization.Core.Models.AccountModels;
using InnoClinic.Authorization.DataAccess.Context;
using InnoClinic.Authorization.DataAccess.Repositories;
using Microsoft.EntityFrameworkCore;
using Testcontainers.MsSql;

namespace InnoClinic.Authorization.TestSuiteNUnit.RepositoryTests;

class AccountRepositoryTests
{
    private MsSqlContainer _dbContainer;
    private InnoClinicAuthorizationDbContext _context;
    private AccountRepository _repository;

    private AccountEntity account;
    private AccountEntity account1;

    [SetUp]
    public async Task Setup()
    {
        account = new AccountEntity
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

        account1 = new AccountEntity
        {
            Id = Guid.NewGuid(),
            Email = "user1@example.com",
            Password = "Password1",
            PhoneNumber = "PhoneNumber1",
            Role = RoleEnum.Receptionist,
            RefreshToken = "RefreshToken1",
            RefreshTokenExpiryTime = DateTime.UtcNow,
            IsEmailVerified = true,
            PhotoId = "PhotoId1",
            CreateBy = RoleEnum.Receptionist.ToString(),
            CreateAt = DateTime.UtcNow,
            UpdateBy = RoleEnum.Receptionist.ToString(),
            UpdateAt = DateTime.UtcNow,
        };

        _dbContainer = new MsSqlBuilder()
            .WithImage("mcr.microsoft.com/mssql/server:2022-latest")
            .WithName("MsSqlTestContainer")
            .WithPassword("Password12345")
            .WithPortBinding(1433)
            .Build();

        await _dbContainer.StartAsync();

        var options = new DbContextOptionsBuilder<InnoClinicAuthorizationDbContext>()
            .UseSqlServer(_dbContainer.GetConnectionString())
            .Options;

        _context = new InnoClinicAuthorizationDbContext(options);

        await _context.Database.EnsureCreatedAsync();

        _repository = new AccountRepository(_context);

    }

    [TearDown]
    public async Task TearDown()
    {
        await _context.DisposeAsync();
        await _dbContainer.StopAsync();
        await _dbContainer.DisposeAsync();
    }

    [Test]
    public async Task CreateAsync_AddsAccountToDatabase()
    {
        // Act
        await _repository.CreateAsync(account);

        var result = await _context.Accounts.FindAsync(account.Id);

        // Assert
        Assert.IsNotNull(result);
        Assert.AreEqual(account.Email, result.Email);
    }

    [Test]
    public async Task GetAllAsync_ShouldReturnsAllAccounts()
    {
        // Arrange
        await _repository.CreateAsync(account);
        await _repository.CreateAsync(account1);

        // Act
        var result = await _repository.GetAllAsync();

        // Assert
        Assert.AreEqual(2, result.Count());
    }

    [Test]
    public async Task GetByIdAsync_ReturnsAccount_WhenExists()
    {
        // Arrange
        await _repository.CreateAsync(account);

        // Act
        var result = await _repository.GetByIdAsync(account.Id);

        // Assert
        Assert.IsNotNull(result);
        Assert.AreEqual(account.Email, result.Email);
    }

    [Test]
    public void GetByIdAsync_ThrowsException_WhenNotFound()
    {
        // Assert
        Assert.ThrowsAsync<ExceptionWithStatusCode>(async () => await _repository.GetByIdAsync(Guid.NewGuid()));
    }

    [Test]
    public async Task EmailExistsAsync_ReturnsTrue_WhenEmailExists()
    {
        // Arrange
        await _repository.CreateAsync(account);

        // Act
        var result = await _repository.IsEmailAvailableAsync(account.Email);

        // Assert
        Assert.IsFalse(result);
    }

    [Test]
    public async Task EmailExistsAsync_ReturnsFalse_WhenEmailDoesNotExist()
    {
        // Act
        var result = await _repository.IsEmailAvailableAsync("nonexistent@example.com");

        // Assert
        Assert.IsTrue(result);
    }

    [Test]
    public async Task GetByEmailAsync_ReturnsAccount_WhenExists()
    {
        // Arrange
        await _repository.CreateAsync(account);

        // Act
        var result = await _repository.GetByEmailAsync(account.Email);

        // Assert
        Assert.IsNotNull(result);
        Assert.AreEqual(account.Email, result.Email);
    }

    [Test]
    public void GetByEmailAsync_ThrowsException_WhenNotFound()
    {
        // Assert
        Assert.ThrowsAsync<ExceptionWithStatusCode>(async () => await _repository.GetByEmailAsync("nonexistent@example.com"));
    }

    [Test]
    public async Task GetByIdsAsync_ReturnsCorrectAccounts()
    {
        // Arrange
        await _repository.CreateAsync(account);
        await _repository.CreateAsync(account1);

        // Act
        var accountIds = new List<Guid> { account.Id, account1.Id };
        var expectedAccountsCount = 2;

        var accounts = await _repository.GetByIdAsync(accountIds);

        // Assert
        Assert.NotNull(accounts);
        Assert.AreEqual(expectedAccountsCount, accounts.Count);
    }

    [Test]
    public async Task UpdateAsync_UpdatesAccount()
    {
        // Arrange
        await _repository.CreateAsync(account);

        // Act
        account.Email = "updated@example.com";
        await _repository.UpdateAsync(account);

        var result = await _repository.GetByIdAsync(account.Id);

        // Assert
        Assert.AreEqual("updated@example.com", result.Email);
    }

    [Test]
    public async Task DeleteAsync_DeletesEntity()
    {
        // Arrange
        await _repository.CreateAsync(account);

        // Act
        await _repository.DeleteAsync(account);

        // Assert
        Assert.ThrowsAsync<ExceptionWithStatusCode>(async () => await _repository.GetByIdAsync(account.Id));
    }

    [Test]
    public async Task GetByRefreshTokenAsync_ExistingRefreshToken_ReturnsAccount()
    {
        // Arrange
        await _repository.CreateAsync(account);

        // Act
        var result = await _repository.GetByRefreshTokenAsync(account.RefreshToken);

        // Assert
        Assert.AreEqual(result.Id, account.Id);
    }

    [Test]
    public void GetByRefreshTokenAsync_NonExistingRefreshToken_ThrowsDataRepositoryException()
    {
        // Arrange
        var refreshToken = "nonExistingRefreshToken";

        // Act & Assert
        var exception = Assert.ThrowsAsync<ExceptionWithStatusCode>(
            async () => await _repository.GetByRefreshTokenAsync(refreshToken));

        // Assert
        Assert.AreEqual($"Account with refresh token '{refreshToken}' not found.", exception.Message);
    }
}
