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
        [Route("register")]
        public async Task<ActionResult> CreateAccountAsync(AccountRequest accountRequest)
        {
            var (accessToken, refreshToken, message) = await _accountService
                .CreateAccountAsync(accountRequest.Email, accountRequest.Password, accountRequest.PhoneNumber, Url);

            return Ok(new { accessToken, refreshToken, message });
        }

        [HttpPost]
        [Route("refresh")]
        public async Task<ActionResult> RefreshTokenAsync(RefreshTokenRequest refreshTokenRequest)
        {
            var (accessToken, refreshToken) = await _accountService
                .RefreshTokenAsync(refreshTokenRequest.AccessToken,
                 refreshTokenRequest.RefreshToken);

            return Ok(new { accessToken, refreshToken });
        }

        [HttpGet]
        [Route("ConfirmEmail")]
        public async Task<IActionResult> ConfirmEmailAsync(string accountId, string token)
        {
            if (string.IsNullOrEmpty(accountId) || string.IsNullOrEmpty(token))
            {
                return BadRequest("Account ID and Token are required.");
            }

            var result = await _accountService.ConfirmEmailAsync(Guid.Parse(accountId), token);

            var htmlContent = $@"
                <!DOCTYPE html>
                <html lang='en'>
                <head>
                    <meta charset='UTF-8'>
                    <meta name='viewport' content='width=device-width, initial-scale=1.0'>
                    <title>Подтверждение аккаунта</title>
                    <style>
                        body {{
                            font-family: Arial, sans-serif;
                            background-color: #f4f4f4;
                            margin: 0;
                            padding: 0;
                        }}
                        .container {{
                            max-width: 600px;
                            margin: 0 auto;
                            background-color: #ffffff;
                            padding: 20px;
                            border-radius: 8px;
                            box-shadow: 0 0 10px rgba(0, 0, 0, 0.1);
                        }}
                        h1 {{
                            color: #333333;
                        }}
                        p {{
                            color: #555555;
                            line-height: 1.6;
                        }}
                        .footer {{
                            margin-top: 20px;
                            text-align: center;
                            color: #999999;
                            font-size: 12px;
                        }}
                        .button {{
                            display: inline-block;
                            padding: 10px 20px;
                            margin-top: 20px;
                            background-color: #007BFF;
                            color: #ffffff !important; /* Убедимся, что цвет текста всегда белый */
                            text-decoration: none;
                            border-radius: 5px;
                            font-weight: bold; /* Добавим жирный шрифт для лучшей читаемости */
                        }}
                        .button:hover {{
                            background-color: #0056b3; /* Цвет при наведении курсора */
                        }}
                    </style>
                </head>
                <body>
                    <div class='container'>
                        <h1>Подтверждение электронной почты'</h1>
                        {(result ? "<p>Ваша почта успешно подтверждена!</p>" : "<p>Произошла ошибка при подтверждении вашей почты. Пожалуйста, попробуйте еще раз.</p>")}
                        <p><a href='https://www.youtube.com/' class='button'>Вернуться на сайт</a></p>
                        <div class='footer'>
                            С уважением,<br>
                            Администрация сайта InnoClinic
                        </div>
                    </div>
                </body>
                </html>";

            return Content(htmlContent, "text/html");
        }
    }
}
