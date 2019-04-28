using System;


namespace Task4
{
    public class Program
    {
        static void Main(string[] args)
        {
            var a = -1;
            var mod = 751;
            var p = (62, 372);
            var q = (70, 195);
            var r = (67, 84);
            Console.WriteLine($"P = {p}");
            Console.WriteLine($"Q = {q}");
            Console.WriteLine($"R = {r} \n");
            var rmin = (r.Item1, -r.Item2);

            var buf1 = p.Double(a, mod);
            var buf2 = q.Double(a, mod).Add(q, mod);
            var buf3 = rmin;
            var res = buf1.Add(buf2, mod).Add(buf3, mod);
            Console.WriteLine($"2P + 3Q - R = {res}");
        }
        
    }
}
