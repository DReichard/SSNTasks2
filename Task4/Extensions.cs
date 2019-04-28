using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Task4
{
    public static class Extensions
    {
        public static ValueTuple<int, int> Add(this ValueTuple<int, int> p1, ValueTuple<int, int> p2, int mod)
        {
            var x1 = p1.Item1;
            var y1 = p1.Item2;
            var x2 = p2.Item1;
            var y2 = p2.Item2;

            var lambda = MathMod(MathMod(y2 - y1, mod) * ModInverse(MathMod(x2 - x1, mod), mod), mod);
            var x3 = MathMod(((int)BigInteger.ModPow(lambda, 2, mod) - x1 - x2), mod);
            var y3 = MathMod((lambda * MathMod((x1 - x3), mod) - y1), mod);
            return (x3, y3);
        }

        public static ValueTuple<int, int> Double(this ValueTuple<int, int> p1, int a, int mod)
        {
            var x1 = p1.Item1;
            var y1 = p1.Item2;
            var x3 = 0;
            var y3 = 0;
            var lambda = MathMod(MathMod((3 * (int)BigInteger.ModPow(x1, 2, mod) + a),mod) * ModInverse((2 * y1), mod), mod);
            x3 = MathMod(((int)BigInteger.ModPow(lambda, 2, mod) - x1 - x1), mod);
            y3 = MathMod((MathMod(lambda * MathMod((x1 - x3), mod), mod) - y1), mod);
            return (x3, y3);
        }
        
        private static int ModInverse(int a, int n)
        {
            int i = n, v = 0, d = 1;
            while (a > 0)
            {
                int t = i / a, x = a;
                a = i % x;
                i = x;
                x = d;
                d = v - t * x;
                v = x;
            }
            v %= n;
            if (v < 0) v = (v + n) % n;
            return v;
        }

        static int MathMod(int a, int b)
        {
            var res = (Math.Abs(a * b) + a) % b;
            return res;
        }
    }
}
