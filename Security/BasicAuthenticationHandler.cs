using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Options;
using System.Net;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text;
using System.Text.Encodings.Web;
using ToDoListWithUsersApi.Controllers;
using ToDoListWithUsersApi.Services;

namespace ToDoListWithUsersApi.Security
{
    public class BasicAuthenticationHandler : AuthenticationHandler<AuthenticationSchemeOptions>
    {
        private readonly IUserService _userService;

        public BasicAuthenticationHandler(
            IUserService userService,
            IOptionsMonitor<AuthenticationSchemeOptions> options,
            ILoggerFactory logger,
            UrlEncoder encoder,
            ISystemClock clock

            ) : base(options, logger, encoder, clock)
        {
            _userService = userService;
        }

        protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            var path = Request.Path.ToString();

            if (path == $"/api/User/AddUser")
            {
                return AuthenticateResult.NoResult();
            }

            string username;
            Guid userId;
            try
            {
                var authHeader = AuthenticationHeaderValue.Parse(Request.Headers["Authorization"]);
                var decoded = Encoding.UTF8.GetString(Convert.FromBase64String(authHeader.Parameter)).Split(':');

                username = decoded[0];
                var password = decoded[1];

                User? user = await _userService.AuthenticateUser(username, password);

                if (user == null)
                {
                    throw new UnauthorizedAccessException();
                }

                userId = user.Id;
            }
            catch (Exception)
            {
                return AuthenticateResult.Fail("Invalid login");
            }

            var claims = new[] { new Claim(ClaimTypes.Name, username), new Claim("UserId", userId.ToString()) };
            var identity = new ClaimsIdentity(claims, Scheme.Name);
            var principal = new ClaimsPrincipal(identity);
            var ticket = new AuthenticationTicket(principal, Scheme.Name);

            return AuthenticateResult.Success(ticket);
        }
    }

}
