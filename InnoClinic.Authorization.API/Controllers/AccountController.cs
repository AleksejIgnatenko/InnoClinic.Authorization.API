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
            return Ok(new { isEmailAvailability });
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<AccountResponse>>> GetAllAccountsAsync()
        {
            var accounts = await _accountService.GetAllAccountsAsync();

            var accountResponses = accounts.Select(account => new AccountResponse(
                account.Id,
                account.Email,
                account.Password,
                account.PhoneNumber,
                account.Role.ToString(),
                account.IsEmailVerified,
                account.PhotoId,
                account.CreateBy,
                account.CreateAt,
                account.UpdateBy,
                account.UpdateAt
            )).ToList();

            return Ok(accountResponses);
        }

        [HttpGet("account-by-account-id-from-token")]
        public async Task<ActionResult<AccountResponse>> GetAccountByAccountIdFromTokenAsync()
        {
            var token = HttpContext.Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
            var account = await _accountService.GetAccountByIdAsync(token);

            var accountResponse = new AccountResponse(
                account.Id,
                account.Email,
                account.Password,
                account.PhoneNumber,
                account.Role.ToString(),
                account.IsEmailVerified,
                account.PhotoId,
                account.CreateBy,
                account.CreateAt,
                account.UpdateBy,
                account.UpdateAt
            );

            return Ok(accountResponse);
        }

        [HttpGet("accounts-by-ids")]
        public async Task<ActionResult> GetAccountsByIdsAsync([FromBody] List<Guid> accountIds)
        {
            var accounts = await _accountService.GetAccountsByIdsAsync(accountIds);

            var accountResponses = accounts.Select(account => new AccountResponse(
                account.Id,
                account.Email,
                account.Password,
                account.PhoneNumber,
                account.Role.ToString(),
                account.IsEmailVerified,
                account.PhotoId,
                account.CreateBy,
                account.CreateAt,
                account.UpdateBy,
                account.UpdateAt
            )).ToList();

            return Ok(accountResponses);
        }
    }
}
