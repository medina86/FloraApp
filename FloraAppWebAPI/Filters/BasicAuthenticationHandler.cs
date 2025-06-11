using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Logging;
using System.Text;
using System.Text.Encodings.Web;
using System.Security.Claims;
using System.Net.Http.Headers;
using FloraApp.Services;
using FloraApp.Model;
using FloraApp.Services.Interfaces;

namespace FloraAppWebAPI.Filters
{
    public class BasicAuthenticationHandler : AuthenticationHandler<AuthenticationSchemeOptions>
    {
        private readonly IUserService _userService;

        public BasicAuthenticationHandler(
            IOptionsMonitor<AuthenticationSchemeOptions> options,
            ILoggerFactory logger,
            UrlEncoder encoder,
            ISystemClock clock,
            IUserService userService)
            : base(options, logger, encoder, clock)
        {
            _userService = userService ?? throw new ArgumentNullException(nameof(userService));
        }

        protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            if (!Request.Headers.ContainsKey("Authorization"))
                return AuthenticateResult.NoResult();

            var authHeader = AuthenticationHeaderValue.Parse(Request.Headers["Authorization"]);
            
            if (authHeader.Scheme != "Basic")
                return AuthenticateResult.Fail("Invalid authentication scheme");

            if (string.IsNullOrEmpty(authHeader.Parameter))
                return AuthenticateResult.Fail("Missing credentials");

            try
            {
                var credentialsBytes = Convert.FromBase64String(authHeader.Parameter);
                var credentials = Encoding.UTF8.GetString(credentialsBytes).Split(':');
                
                if (credentials.Length != 2)
                    return AuthenticateResult.Fail("Invalid credentials format");

                var username = credentials[0];
                var password = credentials[1];

                var user = await _userService.AuthenticateAsync(new FloraApp.Model.Requests.LoginRequest 
                { 
                    Username = username, 
                    Password = password 
                });

                if (user == null)
                    return AuthenticateResult.Fail("Invalid username or password");

                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                    new Claim(ClaimTypes.Name, user.Username),
                    new Claim(ClaimTypes.Email, user.Email),
                    new Claim("IsAdmin", user.IsAdmin.ToString())
                };

                var identity = new ClaimsIdentity(claims, Scheme.Name);
                var principal = new ClaimsPrincipal(identity);
                var ticket = new AuthenticationTicket(principal, Scheme.Name);

                return AuthenticateResult.Success(ticket);
            }
            catch (Exception ex)
            {
                return AuthenticateResult.Fail($"Authentication failed: {ex.Message}");
            }
        }
    }
} 