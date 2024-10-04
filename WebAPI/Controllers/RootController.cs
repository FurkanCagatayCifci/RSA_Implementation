using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers
{
	[ApiController]
	[Route("api")]
	public class RootController : ControllerBase
	{
		[HttpGet]
		[Route("index")]
		public IActionResult Index()
		{
			return Ok();
		}
	}
}
