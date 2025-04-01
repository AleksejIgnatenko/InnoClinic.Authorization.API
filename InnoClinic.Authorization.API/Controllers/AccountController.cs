using InnoClinic.Authorization.Application.Services;
using InnoClinic.Authorization.Core.Models.AccountModels;
using InnoClinic.Authorization.Infrastructure.Jwt;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace InnoClinic.Authorization.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AccountController : ControllerBase
    {
        private readonly IAccountService _accountService;
        private readonly JwtOptions _jwtOptions;

        public AccountController(IAccountService accountService, IOptions<JwtOptions> jwtOptions)
        {
            _accountService = accountService;
            _jwtOptions = jwtOptions.Value;
        }

        [HttpPost]
        [Route("sign-up")]
        public async Task<ActionResult> CreateAccountAsync([FromBody] RegisterAccountRequest accountRequest)
        {
            var (accessToken, refreshToken) = await _accountService
                .CreateAccountAsync(accountRequest.Email, accountRequest.Password, Url);
            var accessTokenCookieOptions = new CookieOptions
            {
                Secure = true,
                SameSite = SameSiteMode.Strict,
                Expires = DateTimeOffset.UtcNow.AddMinutes(_jwtOptions.AccessTokenExpirationMinutes),
                Path = "/",
                Domain = "localhost",
            };

            var refreshTokenCookieOptions = new CookieOptions
            {
                Secure = true,
                HttpOnly = true,
                SameSite = SameSiteMode.Strict,
                Expires = DateTimeOffset.UtcNow.AddDays(_jwtOptions.RefreshTokenExpirationDays),
                Path = "/",
                Domain = "localhost",
            };

            Response.Cookies.Append("accessToken", accessToken, accessTokenCookieOptions);
            Response.Cookies.Append("refreshToken", refreshToken, refreshTokenCookieOptions);

            return Ok();
        }

        [HttpPost]
        [Route("sign-in")]
        public async Task<ActionResult> LoginAsync([FromBody] SignInModelRequest signInModelRequest)
        {
            var (accessToken, refreshToken) = await _accountService.LoginAsync(signInModelRequest.Email, signInModelRequest.Password);

            var accessTokenCookieOptions = new CookieOptions
            {
                Secure = true,
                SameSite = SameSiteMode.Strict,
                Expires = DateTimeOffset.UtcNow.AddMinutes(_jwtOptions.AccessTokenExpirationMinutes),
                Path = "/",
                Domain = "localhost",
            };

            var refreshTokenCookieOptions = new CookieOptions
            {
                Secure = true,  
                HttpOnly = true,
                SameSite = SameSiteMode.Strict, 
                Expires = DateTimeOffset.UtcNow.AddDays(_jwtOptions.RefreshTokenExpirationDays),
                Path = "/", 
                Domain = "localhost",
            };

            Response.Cookies.Append("accessToken", accessToken, accessTokenCookieOptions);
            Response.Cookies.Append("refreshToken", refreshToken, refreshTokenCookieOptions);

            return Ok();
        }

        [HttpPost]
        [Route("refresh")]
        public async Task<ActionResult> RefreshTokenAsync()
        {
            if (!Request.Cookies.TryGetValue("refreshToken", out var refreshToken))
            {
                return BadRequest(new { Message = "Refresh token is missing" });
            }
            var (accessToken, newRefreshToken) = await _accountService.RefreshTokenAsync(refreshToken);

            var accessTokenCookieOptions = new CookieOptions
            {
                Secure = true,
                SameSite = SameSiteMode.Strict,
                Expires = DateTimeOffset.UtcNow.AddMinutes(_jwtOptions.AccessTokenExpirationMinutes),
                Path = "/",
                Domain = "localhost",
            };

            var refreshTokenCookieOptions = new CookieOptions
            {
                Secure = true,
                HttpOnly = true,
                SameSite = SameSiteMode.Strict,
                Expires = DateTimeOffset.UtcNow.AddDays(_jwtOptions.RefreshTokenExpirationDays),
                Path = "/",
                Domain = "localhost",
            };

            Response.Cookies.Append("accessToken", accessToken, accessTokenCookieOptions);
            Response.Cookies.Append("refreshToken", newRefreshToken, refreshTokenCookieOptions);

            return Ok();
        }

        [HttpPost("accounts-by-ids")]
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
                account.CreateBy.ToString(),
                account.CreateAt,
                account.UpdateBy.ToString(),
                account.UpdateAt
            )).ToList();

            return Ok(accountResponses);
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
            return Redirect("http://localhost:4000/create-patient-profile");
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
                account.CreateBy.ToString(),
                account.CreateAt,
                account.UpdateBy.ToString(),
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
                account.CreateBy.ToString(),
                account.CreateAt,
                account.UpdateBy.ToString(),
                account.UpdateAt
            );

            return Ok(accountResponse);
        }
    }
}
