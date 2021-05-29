using System;

namespace TI_Lab_2
{
	class Key
	{
		private readonly Random random = new Random();
		private readonly long[] numbers = { 17, 257, 65537 };

		public long Module { get; private set; }
		public long PrivateKey { get; private set; }
		public long PublicKey { get; private set; }

		private long FindSimpleNumber()
		{
			bool isSimple = false;
			long simpleNum = 0;

			while (!isSimple)
			{
				simpleNum = random.Next(10000, 20000);
				isSimple = true;
				for (long i = 2; i < Math.Sqrt(simpleNum) + 1; i++)
				{
					if (simpleNum % i == 0)
					{
						isSimple = false;
						break;
					}
				}
			}

			return simpleNum;
		}

		private long FindOpenExp(int eilerNum)
		{
			Random rand = new Random();
			long openExp;

			openExp = rand.Next(0, numbers.Length);

			return numbers[openExp];
		}

		private void EuclidEx(long a, long b, out long x, out long d)
		{
			long q, r, x1, x2, y1, y2, y;

			x1 = 0; x2 = 1;
			y1 = 1; y2 = 0;

			while (b > 0)
			{
				q = a / b;
				r = a - q * b;
				x = x2 - q * x1;
				y = y2 - q * y1;
				a = b; b = r;
				x2 = x1; x1 = x;
				y2 = y1; y1 = y;
			}

			d = a;
			x = x2;
		}

		private long Reverse(long a, long n)
		{		
			EuclidEx(a, n, out long x, out long d);

			if (d == 1)
			{
				return x;
			}

			return 0;
		}


		public void CreateKeys()
		{
			long p, q, eulerFunc;

			p = FindSimpleNumber();
			q = FindSimpleNumber();

			if (p == q)
			{
				CreateKeys();

				return;
			}

			Module = p * q;

			eulerFunc = (p - 1) * (q - 1);

			PublicKey = FindOpenExp((int)eulerFunc);

			PrivateKey = Reverse(PublicKey, eulerFunc);

			if (PrivateKey < 0)
			{
				PrivateKey += eulerFunc;
			}
			if (PrivateKey == 0)
			{
				CreateKeys();
			}
		}

	}
}