using CommandLine;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using static Task2.Options;

namespace Task2
{
    class Program
    {
        static int Main(string[] args)
        {
            Thread.CurrentThread.CurrentUICulture = new CultureInfo("en-us");
            return Parser.Default.ParseArguments<SignatureOptions, VerifyOptions>(args)
                .MapResult(
                  (SignatureOptions opts) => MainProcedures.RunSign(opts),
                  (VerifyOptions opts) => MainProcedures.RunVerify(opts),
                  errs => 1);
        }
    }
}
