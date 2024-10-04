using System.Buffers;
using System.IO;
using System.Security.Cryptography;
using System.Text;

using Microsoft.AspNetCore.Http;
using Microsoft.VisualBasic;

namespace Utility
{
	public static class FileOperations
	{

		public static string CreateFileName(string fileName)
		{
			return new String(DateTime.UnixEpoch.ToString() + "_" + fileName);
		}
		private static void CheckFile(string path)
		{
			if (path == null)
			{
				throw new ArgumentNullException("path");
			}
			if (File.Exists(path))
			{
				throw new Exception("File already exists");
			}
		}
		public static async Task<int> SaveFiles(IFormFileCollection formFiles)
		{
			var tasks = new List<Task<int>>();
			foreach (var formFile in formFiles)
			{
				tasks.Add(SaveFile(formFile));
			}
			new Task(() =>
			{
				while (tasks.FindAll(f => f.IsCompleted == false).Any())
				{

				}
			}).Wait(TimeSpan.FromSeconds(10));
			return tasks.FindAll(f => f.IsCompleted == true).Count;
		}
		public static async Task<int> SaveFile(IFormFile file)
		{
			try
			{
				string fileName = CreateFileName(file.FileName);
				CheckFile(fileName);
				File.Create(Core.ProjectSystem.UploadSaveLocation + fileName).Close();
				return 0;
			}
			catch (Exception ex)
			{
				return -1;
			}
		}
	}
	public static class Helper
	{
		public static int GetBitLengthASCII(string str)
		{
			return Encoding.ASCII.GetBytes(str).Length * 8;
		}
		public static int GetBitLengthUTF8(string str)
		{
			return Encoding.UTF8.GetBytes(str).Length * 8;
		}
		public static long GetRandomPrime(int length)
		{
			int counter = 0;
			var bytes = new byte[length - 1];
			long min = (long)Math.Pow(10, length - 1) - 1;
			long max = (long)Math.Pow(10, length) - 1;
			while (counter < 10000)
			{
				
				long x = Random.Shared.NextInt64(min, max);
				if (IsPrime(x))
					return (long)x;
				counter++;
			}
			throw new ArgumentOutOfRangeException();
		}

		public static bool IsPrime(long number)
		{
			//trial division
			for (var i = 2; i <= Math.Sqrt(number); i++)
			{
				if (number % i == 0)
				{
					return false;
				}

			}
			return true;
		}
	}
}
