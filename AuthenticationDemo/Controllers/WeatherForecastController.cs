using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AuthenticationDemo.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [Authorize]
    public class WeatherForecastController : ControllerBase
    {
        [HttpGet]
        public ActionResult<string> GetPeople()
        {
            return Ok("Authenticated User");
        }
    }
}