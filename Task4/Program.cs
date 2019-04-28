using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using static Task4.Extensions;

namespace Task4
{
    public class Program
    {
        static void Main(string[] args)
        {
            var a = -1;
            var mod = 751;
            var p = (58, 139);
            var q = (67, 667);
            var r = (82, 481);
            var rmin = (r.Item1, -r.Item2);

            var buf1 = p.Double(a, mod);
            var buf2 = q.Double(a, mod).Add(q, mod);
            var buf3 = rmin;
            var res = buf1.Add(buf2, mod).Add(buf3, mod);
        }
        
    }
}
