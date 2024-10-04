using System.Numerics;
using System.Text;


using Utility.Algorithm;
public class Program
{

	public static void Main(string[] args)
	{
		Main1(args);
	}


	public static void Main2(string[] args)
	{
		Console.WriteLine(Utility.Helper.Helper.ModuloFermat(7, -1, 55));
		Console.WriteLine(Utility.Helper.Helper.ModuloEuler(7, -1, 55));

	}

	public static void Main1(string[] args)
	{
		long p, q;
		p = Utility.Helper.Helper.GetRandomPrime(2);
		q = Utility.Helper.Helper.GetRandomPrime(2);
		//p = 37;
		//q = 11;
		Console.WriteLine("p : " + p + " q : " + q);
		RSA rsa = new RSA(p, q);
		rsa.e = 7;
		string message = "nabernabernabernabernabernabernabernabernabernabernabernaber";
		//byte[] message = { 7 };
		Console.WriteLine("message :");
		foreach (Int16 c in message)
		{
			Console.Write(String.Format("{0}", (char)c));
		}

		BigInteger[] chiper = rsa.RSAEncryption(Encoding.UTF8.GetBytes(message));
		Console.WriteLine("\nchiper : \r");
		foreach (BigInteger chip in chiper)
		{
			Console.Write(String.Format("{0:d}\t", (ulong)chip));
		}

		List<BigInteger> decoded = rsa.RSADecryption(chiper);
		Console.Write($"\ndecoded chiper : ");
		for (int i = 0; i < decoded.Count; i++)
		{
			Console.Write((char)decoded[i]);
		}
	}
}
//Console.WriteLine(Utility.Helper.IsPrime(26));
//int j = 0;
//List<Exception> es = new List<Exception>();
//for (int i = 0; i < 100; i++)
//{
//	try
//	{
//		ulong number = Utility.Helper.GetRandomPrime(3);
//		Console.WriteLine($"number = {number} length = {number.ToString().Length} bit lenght = {number.ToString().Length * 8}\n");
//		j++;
//	}
//	catch (Exception e)
//	{
//		es.Add(e);
//		j--;
//	}
//}

//Console.WriteLine("Total : " + j);
//Console.WriteLine("Out of Argument : " + es.FindAll(e => e.GetType().Equals(typeof(ArgumentOutOfRangeException))).Count);
//Console.WriteLine("Other : " + es.FindAll(e => !e.GetType().Equals(typeof(ArgumentOutOfRangeException))).Count);