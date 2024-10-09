using System.Buffers;
using System.Diagnostics;
using System.IO;
using System.Numerics;
using System.Security.Cryptography;
using System.Text;

using Microsoft.AspNetCore.Http;
using Microsoft.VisualBasic;

namespace Utility
{
	namespace IO
	{
		public static class FileOperations
		{

			public static string CreateFileName(string fileName)
			{
				return new String((DateTime.UtcNow - DateTime.UnixEpoch).TotalMilliseconds + "_" + fileName);
			}
			public static int CheckPath(string path)
			{
				if (path == null)
				{
					throw new ArgumentNullException("path");
				}
				if (File.Exists(path))
				{
					throw new Exception("File already exists");
				}
				else if (Directory.Exists(path))
				{
					throw new Exception("Directory already exists");
				}
				return 0;
			}
			public static async Task<int> SaveFiles(IFormFileCollection formFiles)
			{
				var tasks = new List<Task<int>>();
				foreach (var formFile in formFiles)
				{
					tasks.Add(SaveFile(formFile));
				}
				var t = new Task(() =>
				{
					while (tasks.FindAll(f => f.IsCompleted == false).Any())
					{

					}
				});
				t.Start();
				t.Wait(TimeSpan.FromSeconds(10));
				return tasks.FindAll(f => f.IsCompleted == true).Count;
			}
			public static async Task<int> SaveFile(IFormFile file)
			{
				string fileName = CreateFileName(file.FileName);
				try
				{
					CheckPath(fileName);
				}
				catch (Exception)
				{
					return -1;
				}
				File.Create($"{Core.Core.UploadSaveLocation}\\{fileName}").Close();
				file.CopyTo(File.Open(fileName, FileMode.Open, FileAccess.ReadWrite, FileShare.None));
				return 0;
			}
			public static async Task<int> SaveFile(IFormFile file, byte[] bytes)
			{
				string fileName = CreateFileName(file.FileName);
				string path = $"{Core.Core.UploadSaveLocation}\\{fileName}";
				CheckPath(path);
				FileStream? fs = System.IO.File.Create(path);
				await fs.WriteAsync(bytes);
				return 0;
			}

			public static async Task<byte[]> ReadIFormFile(IFormFile file)
			{
				Stream fs = file.OpenReadStream();
				List<byte> result = new List<byte>();
				byte[] buffer = new byte[128];
				while (await fs.ReadAsync(buffer) > 0)
				{
					result.AddRange(buffer);
				}
				return result.ToArray();
			}
		}
	}
	namespace Helper
	{
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

			public static long GCD(long x, long y)
			{
				long min = Math.Min(x, y);
				long gcd = 1;

				for (long i = 2; i <= min; i++)
				{
					if (x % i == 0 && y % i == 0)
					{
						gcd = i;
					}
				}
				return gcd;
			}
			public static BigInteger ModuloFermat(BigInteger a, BigInteger b, BigInteger n)
			{
				if (b >= 0)
				{
					return PositiveModPow(a, b, n); // Pozitif üsler için normal üs alma
				}
				else
				{
					BigInteger inverse = ModuloInverseFermat(a, n); // Modüler tersini bul
					return PositiveModPow(inverse, -b, n); // Tersi pozitif üsse göre kullan
				}
			}

			public static BigInteger ModuloEuler(BigInteger a, BigInteger b, BigInteger n)
			{
				if (b >= 0)
				{
					return PositiveModPow(a, b, n); // Pozitif üsler için normal üs alma
				}
				else
				{
					BigInteger inverse = ModuloInverseEuler(a, n); // Modüler tersini bul
					return PositiveModPow(inverse, -b, n); // Tersi pozitif üsse göre kullan
				}
			}


			// Pozitif üsler için hızlı üs alma (modüler üs)
			private static BigInteger PositiveModPow(BigInteger a, BigInteger b, BigInteger n)
			{
				BigInteger result = 1;
				a = a % n; // a mod n

				while (b > 0)
				{
					if ((b & 1) == 1)  // b tekse, sonucu güncelle
					{
						result = (result * a) % n;
					}

					a = (a * a) % n;  // a'nın karesini al ve mod n uygula
					b >>= 1;  // b'yi ikiye böl (bit sağa kaydır)
				}

				return result;
			}

			//private static BigInteger ModuloInverseEuler(BigInteger a, BigInteger n)
			//{
			//	BigInteger t = 0, newT = 1;
			//	BigInteger r = n, newR = a;

			//	while (newR != 0)
			//	{
			//		BigInteger quotient = r / newR;
			//		(t, newT) = (newT, t - quotient * newT);
			//		(r, newR) = (newR, r - quotient * newR);
			//	}

			//	// GCD kontrolü
			//	if (r > 1)
			//		throw new ArgumentException("Modüler ters yok."); // Ters yoksa hata ver

			//	if (t < 0)
			//		t += n; // Pozitif bir modülde dönüş

			//	return t;
			//}


			private static BigInteger ModExpo(BigInteger a, BigInteger exp, BigInteger mod)
			{
				BigInteger result = 1;
				a = a % mod;  // Eğer a moddan büyükse önce modunu alıyoruz

				while (exp > 0)
				{
					// Eğer exp tek ise, sonucu güncelle
					if ((exp % 2) == 1)
						result = (result * a) % mod;

					// exp çiftse, üs değerini ikiye böl ve tabanı karesiyle değiştir
					exp = exp >> 1;  // exp = exp / 2
					a = (a * a) % mod;
				}

				return result;
			}

			// Fermat'ın Küçük Teoremi kullanarak modüler tersini bulma
			private static BigInteger ModuloInverseFermat(BigInteger a, BigInteger p)
			{
				// Modüler ters: a^(p-2) % p
				return ModExpo(a, p - 2, p);
			}


			// Genişletilmiş Öklid Algoritması ile modüler ters bulma

			private static BigInteger ModuloInverseEuler(BigInteger a, BigInteger n)
			{
				BigInteger t = 0, newT = 1;
				BigInteger r = n, newR = a;

				while (newR != 0)
				{
					BigInteger quotient = r / newR;
					(t, newT) = (newT, t - quotient * newT);
					(r, newR) = (newR, r - quotient * newR);
				}

				if (r > 1)
					throw new ArgumentException("Modüler ters yok.");
				if (t < 0)
					t += n;

				return t;
			}
		}
	}
}
