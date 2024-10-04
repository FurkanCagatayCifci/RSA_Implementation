using Microsoft.AspNetCore.Mvc;

using Utility;

namespace WebAPI.Controllers
{
	using System.Web;
	[ApiController]
	[Route("api/fileUpload")]
	public class FileUploadController : RootController
	{
		[HttpPost]
		[Route("upload")]
		public async
			Task<IActionResult> Upload()
		{
			IFormCollection files = Request.Form;
			return Ok();
		}
	}
}
