using System.Reflection;

using Microsoft.AspNetCore.Mvc;

using Utility.Algorithm;
namespace WebAPI
{
	public class Program
	{
		protected internal static RSA rsa = new RSA(
			Utility.Helper.Helper.GetRandomPrime(4),
			Utility.Helper.Helper.GetRandomPrime(4)
		);
		[assembly: Core]
		public static void Main(string[] args)
		{
			InitializeApp();
			var builder = WebApplication.CreateBuilder(args);
			builder.Services.AddControllers();
			builder.Services.AddEndpointsApiExplorer();
			var app = builder.Build();
			app.UseHttpsRedirection();
			app.MapControllers();
			app.UseAuthentication();
			app.UseAuthorization();
			app.UseCors(configurePolicy: options =>
			{
				options.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin();
				options.Build();
			});
			app.Run();
		}
		public static void InitializeApp()
		{
			try
			{
				if(Utility.IO.FileOperations.CheckPath(Core.Core.UploadSaveLocation) == 0)
					Directory.CreateDirectory(Core.Core.UploadSaveLocation);
			}
			catch (Exception)
			{
			}

		}
	}
}