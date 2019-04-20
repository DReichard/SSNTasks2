using CommandLine;

namespace Task1
{
    public class Options
    {
        [Verb("makeselfcert", HelpText = "Make self-signed certificate.")]
        public class MakeSelfCertOptions
        {
            [Option('p', "password", Required = true)]
            public string Password { get; set; }
        }

        [Verb("makecert", HelpText = "Make certificate.")]
        public class MakeCertOptions
        {
            [Option('p', "password", Required = true)]
            public string Password { get; set; }

            [Option('c', "certificate", Required = true)]
            public string CertificatePath { get; set; }
        }

        [Verb("encrypt", HelpText = "Encrypt file.")]
        public class EncryptOptions
        {
            [Option('f', "file", Required = true)]
            public string FilePath { get; set; }

            [Option('o', "output", Required = true)]
            public string Output { get; set; }

            [Option('c', "certificate", Required = true)]
            public string CertificatePath { get; set; }
        }

        [Verb("decrypt", HelpText = "Decrypt file.")]
        public class DecryptOptions
        {
            [Option('f', "file", Required = true)]
            public string FilePath { get; set; }

            [Option('o', "output", Required = true)]
            public string Output { get; set; }

            [Option('c', "certificate", Required = true)]
            public string CertificatePath { get; set; }
        }
    }
}
