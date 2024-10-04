using System.Reflection;

using Microsoft.AspNetCore.Mvc;

public class Program
{
	public static void Main(string[] args)
	{
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

		});
		app.Run();
	}
}
