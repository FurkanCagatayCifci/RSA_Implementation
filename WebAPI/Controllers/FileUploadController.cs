using Microsoft.AspNetCore.Mvc;

using Utility;

namespace WebAPI.Controllers
{
	using System.Buffers;
	using System.Buffers.Text;
	using System.Numerics;
	using System.Text;
	using System.Web;

	using Microsoft.AspNetCore.WebUtilities;
	using Microsoft.Extensions.Primitives;

	[ApiController]
	[Route("api/fileUpload")]
	public class FileUploadController : RootController
	{
		[HttpPost]
		[Route("upload")]
		public async Task<IActionResult> Upload()
		{
			try
			{
				if (Request.Body.CanRead)
				{
					foreach (IFormFile file in Request.Form.Files)
					{
						byte[] formBytes = await Utility.IO.FileOperations.ReadIFormFile(file);
						string formString = Encoding.UTF8.GetString(formBytes);
						string[] chiperString = Encoding.UTF8.GetString(formBytes).Split(",");
						BigInteger[] chiperText = new BigInteger[chiperString.Length];
						Console.Clear();
						for (int i = 0; i < chiperString.Length; i++)
						{
							chiperText[i] = BigInteger.Parse(chiperString[i]);
						}
						Console.WriteLine($"e: {Program.rsa.e} d: {Program.rsa.d} n:{Program.rsa.n}");
						//Console.WriteLine($"{sb} : ");
						Console.WriteLine("ChiperText : ");
						foreach (var i in chiperText)
						{
							Console.Write($"{i}");
						}
						Console.WriteLine("\nPlainText : ");
						List<BigInteger> plainText = Program.rsa.RSADecryption(chiperText);
						foreach (int i in plainText)
						{
							try
							{
								Console.Write($"{(char)i}");
							}
							catch (Exception e)
							{
								;
							}
						}
						Console.WriteLine($"==================");
					}
					return Ok();
				}
				else
					return BadRequest();
			}
			catch (Exception e)
			{
				return BadRequest(e);
			}
		}
	}
}
