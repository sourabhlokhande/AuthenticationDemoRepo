using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AuthenticationDemo.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class WeatherForecastController : ControllerBase
    {
        [HttpGet("Get")]
        public ActionResult<string> GetPeople()
        {
            return Ok("Authenticated User");
        }
    }
}