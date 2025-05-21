using InnoClinic.Authorization.Application.Services;
using InnoClinic.Authorization.Core.Abstractions;
using InnoClinic.Authorization.Core.Models.AccountModels;
using InnoClinic.Authorization.Infrastructure.Jwt;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.Diagnostics.CodeAnalysis;

namespace InnoClinic.Authorization.API.Controllers;

/// <summary>
/// Controller for managing user accounts, including registration, login, email confirmation, and token management.
/// </summary>
[ExcludeFromCodeCoverage]
[ApiController]
[Route("api/[controller]")]
public class AccountController : ControllerBase
{
    private readonly IAccountService _accountService;
    private readonly JwtOptions _jwtOptions;

    /// <summary>
    /// Initializes a new instance of the <see cref="AccountController"/> class.
    /// </summary>
    /// <param name="accountService">Service for account operations.</param>
    /// <param name="jwtOptions">JWT options for token management.</param>
    public AccountController(IAccountService accountService, IOptions<JwtOptions> jwtOptions)
    {
        _accountService = accountService;
        _jwtOptions = jwtOptions.Value;
    }

    /// <summary>
    /// Registers a new user account.
    /// </summary>
    /// <param name="accountRequest">The details of the account to create.</param>
    /// <returns>An <see cref="ActionResult"/> representing the result of the operation.</returns>
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
            Domain = "localhost"
        };

        var refreshTokenCookieOptions = new CookieOptions
        {
            Secure = true,
            HttpOnly = true,
            SameSite = SameSiteMode.Strict,
            Expires = DateTimeOffset.UtcNow.AddDays(_jwtOptions.RefreshTokenExpirationDays),
            Path = "/",
            Domain = "localhost"
        };

        Response.Cookies.Append("accessToken", accessToken, accessTokenCookieOptions);
        Response.Cookies.Append("refreshToken", refreshToken, refreshTokenCookieOptions);

        return Ok();
    }

    /// <summary>
    /// Logs in a user and generates access and refresh tokens.
    /// </summary>
    /// <param name="signInModelRequest">The login credentials.</param>
    /// <returns>An <see cref="ActionResult"/> representing the result of the operation.</returns>
    [HttpPost]
    [Route("sign-in")]
    public async Task<ActionResult> LoginAsync([FromBody] SignInModelRequest signInModelRequest)
    {
        var (accessToken, refreshToken) =
            await _accountService.LoginAsync(signInModelRequest.Email, signInModelRequest.Password);

        var accessTokenCookieOptions = new CookieOptions
        {
            Secure = true,
            SameSite = SameSiteMode.Strict,
            Expires = DateTimeOffset.UtcNow.AddMinutes(_jwtOptions.AccessTokenExpirationMinutes),
            Path = "/",
            Domain = "localhost"
        };

        var refreshTokenCookieOptions = new CookieOptions
        {
            Secure = true,
            HttpOnly = true,
            SameSite = SameSiteMode.Strict,
            Expires = DateTimeOffset.UtcNow.AddDays(_jwtOptions.RefreshTokenExpirationDays),
            Path = "/",
            Domain = "localhost"
        };

        Response.Cookies.Append("accessToken", accessToken, accessTokenCookieOptions);
        Response.Cookies.Append("refreshToken", refreshToken, refreshTokenCookieOptions);

        return Ok();
    }

    /// <summary>
    /// Refreshes the access token using the refresh token from cookies.
    /// </summary>
    /// <returns>An <see cref="ActionResult"/> representing the result of the operation.</returns>
    [HttpPost]
    [Route("refresh")]
    public async Task<ActionResult> RefreshTokenAsync()
    {
        if (!Request.Cookies.TryGetValue("refreshToken", out var refreshToken))
            return BadRequest(new { Message = "Refresh token is missing" });
        var (accessToken, newRefreshToken) = await _accountService.RefreshTokenAsync(refreshToken);

        var accessTokenCookieOptions = new CookieOptions
        {
            Secure = true,
            SameSite = SameSiteMode.Strict,
            Expires = DateTimeOffset.UtcNow.AddMinutes(_jwtOptions.AccessTokenExpirationMinutes),
            Path = "/",
            Domain = "localhost"
        };

        var refreshTokenCookieOptions = new CookieOptions
        {
            Secure = true,
            HttpOnly = true,
            SameSite = SameSiteMode.Strict,
            Expires = DateTimeOffset.UtcNow.AddDays(_jwtOptions.RefreshTokenExpirationDays),
            Path = "/",
            Domain = "localhost"
        };

        Response.Cookies.Append("accessToken", accessToken, accessTokenCookieOptions);
        Response.Cookies.Append("refreshToken", newRefreshToken, refreshTokenCookieOptions);

        return Ok();
    }

    /// <summary>
    /// Retrieves accounts by their IDs.
    /// </summary>
    /// <param name="accountIds">A list of account IDs to retrieve.</param>
    /// <returns>A list of account responses.</returns>
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

    /// <summary>
    /// Confirms the user's email address.
    /// </summary>
    /// <param name="accountId">The ID of the account to confirm.</param>
    /// <param name="token">The confirmation token.</param>
    /// <returns>An <see cref="IActionResult"/> indicating the status of the confirmation.</returns>
    [HttpGet]
    [Route("confirm-email")]
    public async Task<IActionResult> ConfirmEmailAsync(string accountId, string token)
    {
        if (string.IsNullOrEmpty(accountId) || string.IsNullOrEmpty(token))
            return BadRequest("Account Id and Token are required.");

        var result = await _accountService.ConfirmEmailAsync(Guid.Parse(accountId), token);
        return result ? Redirect("http://localhost:4000/create-patient-profile") : BadRequest("Email has not been confirmed");
    }

    /// <summary>
    /// Checks if an email address is available for registration.
    /// </summary>
    /// <param name="email">The email address to check.</param>
    /// <returns>A result indicating the availability of the email.</returns>
    [HttpGet]
    [Route("is-email-available")]
    public async Task<ActionResult> IsEmailAvailableAsync(string email)
    {
        var isEmailAvailability = await _accountService.IsEmailAvailableAsync(email);
        return Ok(new { isEmailAvailability });
    }

    /// <summary>
    /// Retrieves all accounts.
    /// </summary>
    /// <returns>A list of all account responses.</returns>
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

    /// <summary>
    /// Retrieves the account associated with the user token.
    /// </summary>
    /// <returns>The account response associated with the token.</returns>
    [HttpGet("account-by-account-id-from-token")]
    public async Task<ActionResult<AccountResponse>> GetAccountByAccountIdFromTokenAsync()
    {
        var token = HttpContext.Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
        var account = await _accountService.GetAccountByIdFromTokenAsync(token);

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

    /// <summary>
    /// Retrieves the email address of an account by its ID.
    /// </summary>
    /// <param name="id">The ID of the account.</param>
    /// <returns>The email address of the account.</returns>
    [HttpGet("email-by-account-id/{id:guid}")]
    public async Task<ActionResult<AccountResponse>> GetEmailByAccountIdAsync(Guid id)
    {
        var account = await _accountService.GetAccountByIdAsync(id);
        return Ok(account.Email);
    }

    /// <summary>
    /// Logs out the user by deleting the authentication cookies.
    /// </summary>
    /// <returns>An <see cref="ActionResult"/> indicating the status of the logout operation.</returns>
    [HttpDelete]
    [Route("log-out")]
    public ActionResult LogOut()
    {
        Response.Cookies.Delete("accessToken");
        Response.Cookies.Delete("refreshToken");

        return Ok();
    }
}