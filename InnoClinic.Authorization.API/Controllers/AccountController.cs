using InnoClinic.Authorization.API.Contracts;
using InnoClinic.Authorization.Application.Services;
using InnoClinic.Authorization.Core.Contracts;
using Microsoft.AspNetCore.Mvc;

namespace InnoClinic.Authorization.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AccountController : ControllerBase
    {
        private readonly IAccountService _accountService;

        public AccountController(IAccountService accountService)
        {
            _accountService = accountService;
        }

        [HttpPost]
        [Route("sign-up")]
        public async Task<ActionResult> CreateAccountAsync(RegisterAccountRequest accountRequest)
        {
            var (accessToken, refreshToken, message) = await _accountService
                .CreateAccountAsync(accountRequest.Email, accountRequest.Password, Url);

            return Ok(new { accessToken, refreshToken, message });
        }

        [HttpPost]
        [Route("sign-in")]
        public async Task<ActionResult> LoginAsync(string email)
        {
            var (hashPassword, accessToken, refreshToken) = await _accountService
                .LoginAsync(email);

            return Ok(new { hashPassword, accessToken, refreshToken });
        }

        [HttpPost]
        [Route("refresh")]
        public async Task<ActionResult> RefreshTokenAsync(RefreshTokenRequest refreshTokenRequest)
        {
            var (accessToken, refreshToken) = await _accountService
                .RefreshTokenAsync(refreshTokenRequest.RefreshToken);

            return Ok(new { accessToken, refreshToken });
        }

        [HttpGet]
        [Route("confirm-email")]
        public async Task<IActionResult> ConfirmEmailAsync(string accountId, string token)
        {
            if (string.IsNullOrEmpty(accountId) || string.IsNullOrEmpty(token))
            {
                return BadRequest("Account Id and Token are required.");
            }

            var result = await _accountService.ConfirmEmailAsync(Guid.Parse(accountId), token);
            return Redirect("http://localhost:4000/createProfile");
        }

        [HttpGet]
        [Route("email-exists")]
        public async Task<ActionResult> EmailExistsAsync(string email)
        {
            var isEmailAvailability = await _accountService.EmailExistsAsync(email);
            return Ok(new {  isEmailAvailability });
        }

        [HttpGet("get-all")]
        public async Task<ActionResult> GetAllAccountsAsync()
        {
            return Ok(await _accountService.GetAllAccountsAsync());
        }
    }
}
