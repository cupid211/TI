using System;
using System.Numerics;

namespace LW_2.Encryption
{
    public class RSA
    {
        public long p, q, r, e, d;

        public RSA(bool isUserInput, long pUser, long qUser, long eUser)
        {
            Initialize(isUserInput, pUser, qUser, eUser);
        }

        private void Initialize(bool isUserInput, long pUser, long qUser, long eUser)
        {
            if (isUserInput)
            {
                if (PrimeNumber.IsPrime(pUser) && PrimeNumber.IsPrime(qUser))
                {
                    p = pUser;
                    q = qUser;
                }
                else
                    throw new ArgumentException("Invalid RSA parameters");
            }
            else
            {
                p = PrimeNumber.Generate();
                q = PrimeNumber.Generate();
            }

            r = p * q;
            var fi = (p - 1) * (q - 1);

            e = isUserInput ? eUser : GetPublicPartKey(fi);
            d = GetPrivatePartKey(e, fi);
        }

        private long GetPublicPartKey(long fi)
        {
            long e = fi - 1;

            while (true)
            {
                if (PrimeNumber.IsPrime(e) && e < fi &&
                    BigInteger.GreatestCommonDivisor(new BigInteger(e), new BigInteger(fi)) == BigInteger.One)
                    break;

                --e;
            }

            return e;
        }

        private long GetPrivatePartKey(long e, long fi)
        {
            return EuclidAdvanced(e, fi);
        }

        public string Encrypt(string plaintText, long e, long r)
        {
            string cipherText = String.Empty;

            foreach (var ch in plaintText)
            {
                int index = ch;
                var number = FastExp(index, e, r);
                cipherText += number.ToString() + ' ';
            }

            return cipherText;
        }

        public string Decrypt(string[] cipherText, long d, long r)
        {
            string plainText = String.Empty;

            foreach (var item in cipherText)
            {
                var value = new BigInteger(Convert.ToInt64(item));
                var number = FastExp(value, d, r);
                plainText += (char) number;
            }

            return plainText;
        }

        private BigInteger FastExp(BigInteger a, BigInteger z, BigInteger n)
        {
            BigInteger a1 = a, z1 = z, x = 1;
            while (z1 != 0)
            {
                while (z1 % 2 == 0)
                {
                    z1 = z1 / 2;
                    a1 = (a1 * a1) % n;
                }

                z1 = z1 - 1;
                x = (x * a1) % n;
            }

            return x;
        }

        private long EuclidAdvanced(long a, long n)
        {
            long d0 = n;
            long d1 = a;
            long y0 = 0;
            long y1 = 1;

            while (d1 > 1)
            {
                long q = d0 / d1;
                long d2 = d0 % d1;
                long y2 = y0 - q * y1;
                d0 = d1;
                d1 = d2;
                y0 = y1;
                y1 = y2;
            }

            return y1 < 0 ? y1 + n : y1;
        }
    }
}