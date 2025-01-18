using AutoMapper;
using Azure.Core;
using InnoClinic.Authorization.API.Contracts;
using InnoClinic.Authorization.Application.Services;
using InnoClinic.Authorization.Core.Contracts;
using InnoClinic.Authorization.Core.Models;
using Microsoft.AspNetCore.Mvc;

namespace InnoClinic.Authorization.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AccountController : ControllerBase
    {
        private readonly IAccountService _accountService;
        private readonly IMapper _mapper;

        public AccountController(IAccountService accountService, IMapper mapper)
        {
            _accountService = accountService;
            _mapper = mapper;
        }

        [HttpPost]
        public async Task<ActionResult> CreateAccountAsync(AccountRequest accountRequest)
        {
            var account = _mapper.Map<AccountModel>(accountRequest);
            var (accessToken, refreshToken) = await _accountService.CreateAccountAsync(account);
            return Ok(new { accessToken, refreshToken });
        }

        [HttpPost]
        [Route("refresh")]
        public async Task<ActionResult> RefreshTokenAsync(RefreshTokenRequest refreshTokenRequest)
        {
            var(accessToken, refreshToken) = await _accountService
                .RefreshTokenAsync(refreshTokenRequest.AccessToken, 
                 refreshTokenRequest.RefreshToken);

            return Ok(new { accessToken, refreshToken });
        }
    }
}
