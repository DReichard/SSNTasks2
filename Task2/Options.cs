using CommandLine;

namespace Task2
{
    public class Options
    {
        [Verb("sign", HelpText = "Sign file.")]
        public class SignatureOptions
        {
            [Option('c', "certificate", Required = true)]
            public string CertificatePath { get; set; }

            [Option('f', "file", Required = true)]
            public string FilePath { get; set; }

            [Option('p', "password", Required = true)]
            public string Password { get; set; }

            [Option('o', "output", Required = true)]
            public string Output { get; set; }
        }

        [Verb("verify", HelpText = "Verify signature.")]
        public class VerifyOptions
        {
            [Option('f', "file", Required = true)]
            public string FilePath { get; set; }

            [Option('s', "signature", Required = true)]
            public string Signature { get; set; }

            [Option('c', "certificate", Required = true)]
            public string CertificatePath { get; set; }

            [Option('p', "password", Required = true)]
            public string Password { get; set; }
        }
    }
}
