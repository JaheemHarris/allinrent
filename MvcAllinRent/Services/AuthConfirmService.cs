using Microsoft.AspNetCore.DataProtection;
using Microsoft.Extensions.Logging;
using MvcAllinRent.Interfaces;

namespace MvcAllinRent.Services
{
    public class AuthConfirmService
    {
        private readonly IDataProtector _protector;
        private readonly ILogger<AuthConfirmService> _logger;
        private readonly TimeSpan _tokenLifetime = TimeSpan.FromHours(1);
        private readonly IEmailService _emailService;

        private const string HtmlTemplate = @"
            <!DOCTYPE html>
            <html lang=""fr"">
            <head>
                <meta charset=""UTF-8"">
                <meta name=""viewport"" content=""width=device-width, initial-scale=1.0"">
                <link href=""https://fonts.googleapis.com/css2?family=Alice&family=League+Spartan:wght@100..900&family=Montserrat:ital,wght@0,100..900;1,100..900&display=swap"" rel=""stylesheet"">
                <title>Confirmation de compte</title>
                <style>
                    body {
                        font-family: 'League Spartan', Arial, sans-serif;
                        background-color: #f9f9f9;
                        margin: 0;
                        padding: 20px;
                    }
                    .site-title {
                        color: #0097b2;
                    }
                    .email-container {
                        max-width: 600px;
                        margin: 0 auto;
                        background-color: #ffffff;
                        padding: 20px;
                        border-radius: 8px;
                        box-shadow: 0 0 10px rgba(0, 0, 0, 0.1);
                    }
                    h1 {
                        color: #333333;
                    }
                    p {
                        color: #555555;
                        line-height: 1.6;
                    }
                    .button {
                        display: inline-block;
                        padding: 10px 20px;
                        background-color: #007bff;
                        color: #ffffff;
                        text-decoration: none;
                        border-radius: 4px;
                    }
                    .footer {
                        margin-top: 20px;
                        font-size: 12px;
                        color: #aaaaaa;
                        text-align: center;
                    }
                </style>
            </head>
            <body>
                <div class=""email-container"">
                    <h3>Bienvenue sur <span class=""site-title"">All-In-Rent</span> !</h3>
                    <p>Votre compte a été créé ! Pour l'activer, veuillez confirmer votre adresse e-mail en cliquant sur le lien ci-dessous :</p>
                    <p><a href=""{0}"" class=""button"">Activer mon compte</a></p>
                    <p>Ce lien expirera dans {1} minutes.</p>
                    <p>Si vous n'avez pas créé de compte, veuillez ignorer cet e-mail.</p>
                    <div class=""footer"">
                        &copy; 2025 All-In-Rent. Tous droits réservés.
                    </div>
                </div>
            </body>
            </html>";

        public AuthConfirmService(IDataProtectionProvider dataProtectionProvider, ILogger<AuthConfirmService> logger, IEmailService emailService)
        {
            _protector = dataProtectionProvider.CreateProtector("AuthConfirmationToken");
            _logger = logger;
            _emailService = emailService;
        }

        public string GenerateToken(string email)
        {
            var payload = $"{email}|{DateTime.UtcNow.Add(_tokenLifetime):O}";
            return _protector.Protect(payload);
        }

        public bool ValidateToken(string token, out string email)
        {
            email = string.Empty;

            try
            {
                var payload = _protector.Unprotect(token);
                var parts = payload.Split('|');
                if (parts.Length != 2)
                {
                    _logger.LogWarning("Invalid token format.");
                    return false;
                }

                email = parts[0];
                if (!DateTime.TryParse(parts[1], out var expiration) || DateTime.UtcNow > expiration)
                {
                    _logger.LogWarning("Token is either invalid or expired.");
                    return false;
                }

                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Token validation failed.");
                return false;
            }
        }

        public async Task SendConfirmationLinkAsync(string email, string confirmationUrl)
        {
            try
            {
                var token = GenerateToken(email);
                var link = string.Format(confirmationUrl, token);
                var htmlBody = string.Format(HtmlTemplate, link, (int)_tokenLifetime.TotalMinutes);

                await _emailService.SendEmailAsync(
                    email,
                    "Activation de compte",
                    htmlBody
                );
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to send confirmation link.");
                throw;
            }
        }
    }
}