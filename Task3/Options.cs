using CommandLine;

namespace Task3
{
    public class Options
    {
        [Verb("test3des", HelpText = "Test 3DES MAC.")]
        public class TripleDesOptions
        {
            [Option('f', "file", Required = true)]
            public string FilePath { get; set; }            
        }

        [Verb("testmd5hmac", HelpText = "Verify HMAC signature.")]
        public class Md5HmacOptions
        {
            [Option('f', "file", Required = true)]
            public string FilePath { get; set; }
        }
    }
}
