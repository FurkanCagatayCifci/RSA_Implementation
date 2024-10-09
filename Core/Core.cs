using System.Configuration;
using System.Numerics;
using System.Reflection;
namespace Core
{
	public sealed class Core
	{
		static Core()
		{
			unsafe
			{
				FormFileReadingBufferSize = (sizeof(BigInteger));
				UploadSaveLocation = Assembly.GetEntryAssembly().Location.Trim().Replace(Assembly.GetEntryAssembly().ManifestModule.Name, "Uploads");
			}
		}

		public static string UploadSaveLocation;
		public static int FormFileReadingBufferSize;
	}
}