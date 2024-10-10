using System.Text.Json.Nodes;

using Core;

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
				KeyPair? obj = new KeyPair()
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
		[HttpGet]
		[Route("maxFileLength")]
		public async Task<IActionResult> GetMaxFileLength()
		{
			try
			{
				return Ok(Core.Core.MaxFileLength);
			}
			catch (Exception e)
			{
				return BadRequest(new Message(e.Message));
			}
		}
	}

}
