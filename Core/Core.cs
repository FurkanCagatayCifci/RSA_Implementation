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
				UploadSaveLocation = Assembly.GetEntryAssembly().Location;
			}
		}

		public static string UploadSaveLocation;
		public static int FormFileReadingBufferSize;
	}
}