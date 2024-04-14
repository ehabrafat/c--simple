using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Primitives;

namespace Conteact_list_exercise.Controllers
{
    [ApiController]
    [Route("auth")]
    public class AuthController : Controller
    {
        private readonly IHttpContextAccessor httpContextAccessor;
        private readonly IDataProtectionProvider dataProtectionProvider;

        public AuthController(IHttpContextAccessor httpContextAccessor, IDataProtectionProvider dataProtectionProvider)
        {
            this.httpContextAccessor = httpContextAccessor;
            this.dataProtectionProvider = dataProtectionProvider;
        }

        Dictionary<string, string> ParseCookies(string cookie)
        {
            var lookup = new Dictionary<string, string>();
            var list = cookie.Split(";");
            foreach (var item in list)
            {
                string key = item.Split("=").First();
                string value = item.Split("=").Last();
                lookup[key.Trim()] = value.Trim();
            }
            return lookup;
        }


        [HttpGet]
        [Route("username")]
        public IActionResult getMe()
        {
            var cookie = httpContextAccessor.HttpContext!.Request.Headers.Cookie;
            Dictionary<string, string> cookieParsed = ParseCookies(cookie);
            var protector = dataProtectionProvider.CreateProtector("auth");
            Console.WriteLine(protector.Unprotect(cookieParsed["user"]));
            return Ok();
        }

        [HttpGet]
        public IActionResult signMe()
        {
            var protector = dataProtectionProvider.CreateProtector("auth");
            httpContextAccessor.HttpContext!.Response.Headers["set-cookie"] = $"user={protector.Protect("ehab")}";
            return Created();
        }
    }
}
