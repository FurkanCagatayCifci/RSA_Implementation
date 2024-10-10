using System.Configuration;
using System.Numerics;
using System.Reflection;
namespace Core
{
	public class Message
	{
		public string? _message { get; set; }
		public bool? _error { get; set; }

		public object? _data { get; set; }
		public Message(string? message, bool? error)
		{
			_message = message;
			_error = error;
		}
		public Message(string? message)
		{
			_message = message;
		}
		public Message(string? message, object? data)
		{
			_message = message;
			_data = data;
		}
	}
	public sealed class Core
	{
		static Core()
		{
			unsafe
			{
				FormFileReadingBufferSize = (sizeof(BigInteger));
				UploadSaveLocation = Assembly.GetEntryAssembly().Location.Trim().Replace(Assembly.GetEntryAssembly().ManifestModule.Name, "Uploads");
				MaxFileLength = 2048;
			}
		}

		public static string UploadSaveLocation;
		public static int FormFileReadingBufferSize;
		public static int MaxFileLength;
	}
}