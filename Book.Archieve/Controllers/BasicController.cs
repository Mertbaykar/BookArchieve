using Book.Archieve.API.Filter;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Book.Archieve.API.Controllers
{
    [ServiceFilter(typeof(ExceptionLoggerFilter))]
    [Route("api/[controller]/[action]")]
    [ApiController]
    public abstract class BasicController : ControllerBase
    {
        public int UserId => GetUserId();

        private int GetUserId()
        {
            return Convert.ToInt32(User.Claims.First(x => x.Type == ClaimTypes.NameIdentifier).Value);
        }
    }
}
