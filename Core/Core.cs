using System.Reflection;

namespace Core
{
	public class Core
	{

	}
	public static class ProjectSystem
	{
		public static string UploadSaveLocation = null;
		static ProjectSystem()
		{
			UploadSaveLocation = Assembly.GetEntryAssembly().Location;
		}
	}
}
