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
	public class RSA
	{
		long p, q, n, phi;

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
		public List<BigInteger> RSADecryption(BigInteger[] bytes)
		{

			List<BigInteger> decrypted = new List<BigInteger>();
			for (int i = 0; i < bytes.Length; i++)
			{
				decrypted.Insert(i, (Utility.Helper.Helper.ModuloFermat(bytes[i], d, n)));
			}
			return decrypted;
		}
	}
}
