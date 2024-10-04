using System.Numerics;
using System.Text;


using Utility.Algorithm;
public class Program
{
	public static void Main(string[] args)
	{
		long p, q;
		p = Utility.Helper.GetRandomPrime(3);
		q = Utility.Helper.GetRandomPrime(3);
		Console.WriteLine("p : " + p + " q : " + q);
		//p = 71;
		//q = 53;
		RSA rsa = new RSA(p, q);
		string message = "nabernabernabernabernabernabernabernabernabernabernabernaber";
		//byte[] message = { 7 };
		BigInteger[] chiper = rsa.RSAEncryption(Encoding.UTF8.GetBytes(message));
		Console.Write(
$"message :");
		foreach (Int16 c in message)
		{
			Console.Write(
		$"{(char)c}");
		}
		Console.Write(
		$"\nchiper : \n"
	);

		foreach (BigInteger chip in chiper)
		{
			Console.Write($"{chip:b}\t");
		}
		Console.Write($"\ndecoded chiper : ");
		List<byte[]> decoded = rsa.RSADecryption(chiper);
		for (int i = 0; i < decoded.Count; i++)
		{
			foreach (byte b in decoded[i])
				Console.Write($"{(char)b}");
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