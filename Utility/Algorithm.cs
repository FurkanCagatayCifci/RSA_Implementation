using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Utility.Algorithm
{
	public class RSA
	{
		long p, q, n, phi;
		BigInteger e, d;
		short blockSize = 10;
		public RSA(long p, long q)
		{
			this.p = p; this.q = q;
			n = p * q;
			phi = (p - 1) * (q - 1);
			long random;
			while (true)
			{
				random = new Random().NextInt64(1, phi);

				if (GCD(x: random, phi) == 1)
				{
					e = random;
					break;
				}
			}
			d = Mod(e, b: -1, phi);
		}
		public long GCD(long x, long y)
		{
			x = 880; y = 7;
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
		public static BigInteger Mod(BigInteger a, BigInteger b, BigInteger n)
		{
			if (b >= 0)
			{
				return PositiveModPow(a, b, n); // Pozitif üsler için normal üs alma
			}
			else
			{
				BigInteger inverse = ModularInverse(a, n); // Modüler tersini bul
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
		//private static BigInteger ModInverse(BigInteger a, BigInteger n)
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


		static BigInteger ModExpo(BigInteger a, BigInteger exp, BigInteger mod)
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
		static BigInteger ModularInverse(BigInteger a, BigInteger p)
		{
			// Modüler ters: a^(p-2) % p
			return ModExpo(a, p - 2, p);
		}


		// Genişletilmiş Öklid Algoritması ile modüler ters bulma
		//private static BigInteger ModInverse(BigInteger a, BigInteger n)
		//{
		//	BigInteger t = 0, newT = 1;
		//	BigInteger r = n, newR = a;

		//	while (newR != 0)
		//	{
		//		BigInteger quotient = r / newR;
		//		(t, newT) = (newT, t - quotient * newT);
		//		(r, newR) = (newR, r - quotient * newR);
		//	}

		//	if (r > 1)
		//		throw new ArgumentException("Modüler ters yok.");
		//	if (t < 0)
		//		t += n;

		//	return t;
		//}

		public BigInteger[] RSAEncryption(byte[] message)
		{
			int numBlocks = (int)Math.Ceiling((double)message.Length / blockSize);
			BigInteger[] encrypted = new BigInteger[numBlocks];

			for (int i = 0; i < numBlocks; i++)
			{
				int start = i * blockSize;
				int length = Math.Min(blockSize, message.Length - start);

				byte[] block = new byte[length];
				Array.Copy(message, start, block, 0, length);
				ulong blockValue = (ulong)BitConverter.ToUInt64(value: block);
				encrypted[i] = Mod(blockValue, e, n);
			}

			return encrypted;
		}
		public List<byte[]> RSADecryption(BigInteger[] bytes)
		{

			List<byte[]> decrypted = new List<byte[]>();
			for (int i = 0; i < bytes.Length; i++)
			{
				decrypted.Insert(i, ((BigInteger)Mod((long)bytes[i], d, n)).ToByteArray());
			}
			return decrypted;
		}
	}
}
