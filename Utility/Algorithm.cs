using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

using Utility.Helper;

namespace Utility.Algorithm
{
	public class KeyPair
	{
		public string x { get; set; }
		public string n { get; set; }
	}

	public class RSA
	{
		public long n;
		long p, q, phi;
		private BigInteger _e, _d;
		public BigInteger e
		{
			get { return _e; }
			set
			{
				_e = value;
				d = Utility.Helper.Helper.ModuloEuler(e, -1, phi);
			}
		}
		public BigInteger d
		{
			get
			{
				return _d;
			}
			set
			{
				_d = value;
			}
		}
		public RSA(long p, long q)
		{
			if (Utility.Helper.Helper.IsPrime(p))
			{
				if (
				Utility.Helper.Helper.IsPrime(q)
				) ;
				else throw new ArgumentException("q is not a prime number");

			}

			else throw new ArgumentException("p is not a prime number");
			this.p = p; this.q = q;
			n = p * q;
			phi = (p - 1) * (q - 1);
			long random;
			while (true)
			{
				random = new Random().NextInt64(1, phi);

				if (Utility.Helper.Helper.GCD(x: random, phi) == 1)
				{
					e = random;
					break;
				}
			}
		}
		public BigInteger[] RSAEncryption(byte[] message)
		{
			BigInteger[] encrypted = new BigInteger[message.Length];
			for (int i = 0; i < message.Length; i++)
			{
				encrypted[i] = Utility.Helper.Helper.ModuloFermat(message[i], e, n);
			}

			return encrypted;
		}
		public List<BigInteger> RSADecryption(byte[] bytes)
		{

			List<BigInteger> decrypted = new List<BigInteger>();
			for (int i = 0, j = 0; i < bytes.Length - 15; i++)
			{
				Span<byte> x = new Span<byte>(bytes, i + 15, 16);
				decrypted.Insert(i, (Utility.Helper.Helper.ModuloFermat(new BigInteger(x), d, n)));
				j = 0;
			}
			return decrypted;
		}
		public List<BigInteger> RSADecryption(BigInteger[] bytes)
		{
			List<BigInteger> decrypted = new List<BigInteger>();
			for (int i = 0; i < bytes.Length - 1; i++)
			{
				decrypted.Insert(i, (Utility.Helper.Helper.ModuloFermat(bytes[i], d, n)));
			}
			return decrypted;
		}
		public char[] RSADecryptionToCharArray(BigInteger[] bytes)
		{
			List<char> decrypted = new List<char>();
			char[] charBuff = new char[1];
			Span<char> buffSpan = new Span<char>(charBuff);
			for (int i = 0; i < bytes.Length - 1; i++)
			{
				try
				{
					BigInteger x = (Utility.Helper.Helper.ModuloFermat(bytes[i], b: d, n));
					char decrypredChar = (char)x;
					decrypted.Insert(i, decrypredChar);
				}
				catch (Exception)
				{

				}

			}
			return decrypted.ToArray();

		}
		public byte[] RSADecryptionToByteArray(BigInteger[] bytes)
		{
			List<byte> decrypted = new List<byte>();
			byte[] byteBuff = new byte[1];
			Span<byte> buffSpan = new Span<byte>(byteBuff);
			for (int i = 0; i < bytes.Length - 1; i++)
			{
				try
				{
					
					BigInteger x = (Utility.Helper.Helper.ModuloFermat(bytes[i], b: d, n));
					byte decrypredChar = (byte)x;
					decrypted.Insert(i, decrypredChar);
				}
				catch (Exception)
				{

				}
			}
			return decrypted.ToArray();

		}

	}
}
