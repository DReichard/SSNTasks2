using CommandLine;
using System.Globalization;
using System.Threading;
using static Task1.Options;

namespace Task1
{
    class Program
    {
        static int Main(string[] args)
        {
            Thread.CurrentThread.CurrentUICulture = new CultureInfo("en-us");
            return Parser.Default.ParseArguments<MakeSelfCertOptions, EncryptOptions, DecryptOptions>(args)
                .MapResult(
                  (MakeSelfCertOptions opts) => MainProcedures.RunMakeSelfCert(opts),
                  (EncryptOptions opts) => MainProcedures.RunEncrypt(opts),
                  (DecryptOptions opts) => MainProcedures.RunDecrypt(opts),
                  errs => 1);
        }
    }
}
