using System.Text.Json.Nodes;

using Microsoft.AspNetCore.Mvc;

using Utility.Algorithm;

namespace WebAPI.Controllers
{
	[ApiController]
	[Route("api")]
	public class RootController : ControllerBase
	{
		[HttpGet]
		[Route("index")]
		public async Task<IActionResult> Index()
		{
			return Ok();
		}
		[HttpGet]
		[Route("getKey")]
		public async Task<IActionResult> GetPublicKey()
		{
			try
			{
				var obj = new KeyPair()
				{
					n = Program.rsa.n.ToString(),
					x = Program.rsa.e.ToString()
				};

				return Ok(obj);
			}
			catch (Exception e)
			{
				return BadRequest(e);
			}
		}
	}

}
