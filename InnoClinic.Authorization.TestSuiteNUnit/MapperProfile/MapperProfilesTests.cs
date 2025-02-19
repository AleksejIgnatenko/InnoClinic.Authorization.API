using AutoMapper;
using InnoClinic.Authorization.Application.MapperProfiles;
using InnoClinic.Authorization.Core.Dto;
using InnoClinic.Authorization.Core.Models;
using Microsoft.AspNetCore.Http.HttpResults;

namespace InnoClinic.Authorization.TestSuiteNUnit.MapperProfile
{
    public class MapperProfilesTests
    {
        private readonly IMapper _mapper;

        public MapperProfilesTests()
        {
            var config = new MapperConfiguration(cfg => cfg.AddProfile<MapperProfiles>());
            _mapper = config.CreateMapper();
        }

        [Test]
        public void Should_Map_AccountModel_To_AccountDto()
        {
            // Arrange
            var accountModel = new AccountModel
            {
                Id = Guid.NewGuid(),
                Email = "test@example.com",
                Password = "Password",
                PhoneNumber = "PhoneNumber",
                Role = Core.Enums.RoleEnum.Receptionist
            };

            // Act
            var accountDto = _mapper.Map<AccountDto>(accountModel);

            // Assert
            Assert.AreEqual(accountModel.Id, accountDto.Id);
            Assert.AreEqual(accountModel.Email, accountDto.Email);
            Assert.AreEqual(accountModel.Password, accountDto.Password);
            Assert.AreEqual(accountModel.PhoneNumber, accountDto.PhoneNumber);
            Assert.AreEqual(accountModel.Role, accountDto.Role);
        }

        [Test]
        public void Should_Map_AccountDto_To_AccountModel()
        {
            // Arrange
            var accountDto = new AccountDto
            {
                Id = Guid.NewGuid(),
                Email = "test@example.com",
                Password = "Password",
                PhoneNumber = "PhoneNumber",
                Role = Core.Enums.RoleEnum.Receptionist
            };

            // Act
            var accountModel = _mapper.Map<AccountModel>(accountDto);

            // Assert
            Assert.AreEqual(accountModel.Id, accountDto.Id);
            Assert.AreEqual(accountModel.Email, accountDto.Email);
            Assert.AreEqual(accountModel.Password, accountDto.Password);
            Assert.AreEqual(accountModel.PhoneNumber, accountDto.PhoneNumber);
            Assert.AreEqual(accountModel.Role, accountDto.Role);
        }
    }
}
