using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Options;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text.Encodings.Web;
using System.Text;
using ToDoListWithUsersApi.Services;
using DataLibrary;
using DataLibrary.Models;

namespace ToDoListWithUsersApi.Security
{
    public class BearerAuthenticationHandler : AuthenticationHandler<AuthenticationSchemeOptions>
    {
        private readonly IUserService _userService;

        public BearerAuthenticationHandler(
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

            if (path == $"/api/User/AddUser" || path == $"/api/User/Login")
            {
                return AuthenticateResult.NoResult();
            }

            Guid userId;
            UserModel user;
            //string username;

            try
            {
                userId = CurrentActive.Id["UserId"];
                user = _userService.GetUser(userId);
            }
            catch (Exception)
            {
                return AuthenticateResult.Fail("Not logged in");
            }

            //Kan man lägga till funktion för att kolla vilken permission level den har här???

            var claims = new[] 
            { 
                new Claim("UserId", userId.ToString()),
                new Claim(ClaimTypes.Role, user.Permission.ToString())
            };
            var identity = new ClaimsIdentity(claims, Scheme.Name);
            var principal = new ClaimsPrincipal(identity);
            var ticket = new AuthenticationTicket(principal, Scheme.Name);

            return AuthenticateResult.Success(ticket);
        }
    }
}
