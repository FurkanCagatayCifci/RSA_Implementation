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
				Console.Clear();
				if (Request.Body.CanRead)
				{
					foreach (IFormFile file in Request.Form.Files)
					{
						byte[] formBytes = await Utility.IO.FileOperations.ReadIFormFile(file);
						string formString = Encoding.UTF8.GetString(formBytes);
						string[] chiperString = Encoding.UTF8.GetString(formBytes).Split(",");
						BigInteger[] chiperText = new BigInteger[chiperString.Length];
						for (int i = 0; i < chiperString.Length; i++)
						{
							try
							{
								chiperText[i] = BigInteger.Parse(chiperString[i]);
							}
							catch (Exception)
							{
								;
							}
						}
						Console.WriteLine($"e: {Program.rsa.e} d: {Program.rsa.d} n:{Program.rsa.n}");
						Console.WriteLine($"File Name : {file.Name}");
						Console.WriteLine("ChiperText : ");
						foreach (var i in chiperText)
						{
							Console.Write($"{i}");
						}
						Console.WriteLine("\nPlainText : ");
						byte[] plainText = Program.rsa.RSADecryptionToByteArray(chiperText);
						foreach (char i in plainText)
						{
							Console.Write($"{i:c}");
						}
						Console.WriteLine($"\n==================");
						try
						{
							await Utility.IO.FileOperations.SaveFile(file, plainText);
						}
						catch (Exception e)
						{
							Console.WriteLine("Exception on File Creation");
							Console.WriteLine(e.Message);
						}
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
