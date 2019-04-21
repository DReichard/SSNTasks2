using CommandLine;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using static Task3.Options;

namespace Task3
{
    class Program
    {
        static int Main(string[] args)
        {
            Thread.CurrentThread.CurrentUICulture = new CultureInfo("en-us");
            return Parser.Default.ParseArguments<TripleDesOptions, Md5HmacOptions>(args)
                .MapResult(
                  (TripleDesOptions opts) => MainProcedures.RunTripleDesTest(opts),
                  (Md5HmacOptions opts) => MainProcedures.RunMd5HmacTest(opts),
                  errs => 1);
        }
    }
}
