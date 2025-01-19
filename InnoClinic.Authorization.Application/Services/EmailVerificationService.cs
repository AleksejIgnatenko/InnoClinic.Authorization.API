using InnoClinic.Authorization.Core.Models;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MailKit.Net.Smtp;
using MimeKit;

namespace InnoClinic.Authorization.Application.Services
{
    public class EmailVerificationService : IEmailVerificationService
    {
        private const string _emai = "innoclinic33@gmail.com";
        private const string _emaiAppPassword = "spaz tebr scpd lahu";

        private readonly IDataProtector _dataProtector;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public EmailVerificationService(IDataProtectionProvider dataProtectionProvider, IHttpContextAccessor httpContextAccessor)
        {
            _dataProtector = dataProtectionProvider.CreateProtector("EmailConfirmation");
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task SendVerificationEmailAsync(AccountModel account, IUrlHelper urlHelper)
        {
            var token = GenerateEmailConfirmationToken(account);

            var callbackUrl = urlHelper.Action(
                "ConfirmEmail",
                "Account",
                new { accountId = account.Id, token = token },
                _httpContextAccessor.HttpContext.Request.Scheme);

            var subject = "Подтвердите ваш аккаунт";
            string message = $@"
            <html>
                <head>
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
                        .footer {{
                            margin-top: 20px;
                            text-align: center;
                            color: #999999;
                            font-size: 12px;
                        }}
                    </style>
                </head>
                <body>
                    <div class='container'>
                        <h1>Подтвердите ваш Email</h1>
                        <p>Здравствуйте!</p>
                        <p>Благодарим вас за регистрацию на нашем сайте. Для завершения процесса подтверждения вашего адреса электронной почты, пожалуйста, перейдите по ссылке ниже:</p>
                        <p><a href='{callbackUrl}' class='button'>Подтвердить аккаунт</a></p>
                        <p>Если вы не регистрировались на нашем сайте, просто проигнорируйте это письмо.</p>
                        <div class='footer'>
                            С уважением,<br>
                            Администрация сайта InnoClinic
                        </div>
                    </div>
                </body>
            </html>";

            await SendEmailAsync(account.Email, subject, message);
        }

        public string ConfirmEmail(string token)
        {
            return _dataProtector.Unprotect(token);
        }

        private string GenerateEmailConfirmationToken(AccountModel account)
        {
            var token = Guid.NewGuid().ToString();
            var protectedToken = _dataProtector.Protect(token);
            return protectedToken;
        }

        private async Task SendEmailAsync(string email, string subject, string message)
        {
            var emailMessage = new MimeMessage();
            emailMessage.From.Add(new MailboxAddress("Администрация сайта", _emai));
            emailMessage.To.Add(new MailboxAddress("", email));
            emailMessage.Subject = subject;
            emailMessage.Body = new TextPart(MimeKit.Text.TextFormat.Html)
            {
                Text = message
            };

            using (var client = new SmtpClient())
            {
                await client.ConnectAsync("smtp.gmail.com", 587, MailKit.Security.SecureSocketOptions.StartTls);
                await client.AuthenticateAsync(_emai, _emaiAppPassword);
                await client.SendAsync(emailMessage);
                await client.DisconnectAsync(true);
            }
        }
    }
}
