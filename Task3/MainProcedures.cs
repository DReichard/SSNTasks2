using System;
using System.Security.Cryptography.X509Certificates;
using System.Security.Cryptography;
using System.Net;
using System.IO;
using static Task3.Options;
using System.Text;

namespace Task3
{
    public static class MainProcedures
    {

        internal static int RunTripleDesTest(TripleDesOptions opts)
        {
            try
            {
                var file = File.ReadAllBytes(opts.FilePath);
                var hardcodedKey = Encoding.UTF8.GetBytes("SecretKeyMorozov");
                var signedBytes = SignBytes3des(hardcodedKey, file);

                Console.WriteLine("Original file:");
                Console.WriteLine(Encoding.UTF8.GetString(file));
                Console.WriteLine();

                Console.WriteLine("Signed bytes:");
                Console.WriteLine(Encoding.UTF8.GetString(signedBytes));
                Console.WriteLine();

                var verdict = VerifyBytes3des(hardcodedKey, signedBytes);
                Console.WriteLine("Integrity:");
                Console.WriteLine(verdict);
                Console.WriteLine();

                signedBytes[15] = 0x6;
                Console.WriteLine("Modified bytes:");
                Console.WriteLine(Encoding.UTF8.GetString(signedBytes));
                Console.WriteLine();

                verdict = VerifyBytes3des(hardcodedKey, signedBytes);
                Console.WriteLine("Integrity:");
                Console.WriteLine(verdict);
            }
            catch (Exception e)
            {
                return -1;
            }
            return 0;
        }

        internal static int RunMd5HmacTest(Md5HmacOptions opts)
        {
            try
            {
                var file = File.ReadAllBytes(opts.FilePath);
                var hardcodedKey = Encoding.UTF8.GetBytes("SecretKeyMorozov");
                var signedBytes = SignBytes3des(hardcodedKey, file);

                Console.WriteLine("Original file:");
                Console.WriteLine(Encoding.UTF8.GetString(file));
                Console.WriteLine();

                Console.WriteLine("Signed bytes:");
                Console.WriteLine(Encoding.UTF8.GetString(signedBytes));
                Console.WriteLine();

                var verdict = VerifyBytes3des(hardcodedKey, signedBytes);
                Console.WriteLine("Integrity:");
                Console.WriteLine(verdict);
                Console.WriteLine();

                signedBytes[15] = 0x6;
                Console.WriteLine("Modified bytes:");
                Console.WriteLine(Encoding.UTF8.GetString(signedBytes));
                Console.WriteLine();

                verdict = VerifyBytes3des(hardcodedKey, signedBytes);
                Console.WriteLine("Integrity:");
                Console.WriteLine(verdict);
            }
            catch (Exception e)
            {
                return -1;
            }
            return 0;
        }

        private static byte[] SignBytes3des(byte[] key, byte[] source)
        {
            using (var hmac = new MACTripleDES(key))
            {
                using (var inStream = new MemoryStream(source))
                {
                    using (var outStream = new MemoryStream())
                    {
                        var hashValue = hmac.ComputeHash(inStream);
                        inStream.Position = 0;
                        outStream.Write(hashValue, 0, hashValue.Length);
                        int bytesRead;
                        var buffer = new byte[1024];
                        do
                        {
                            bytesRead = inStream.Read(buffer, 0, 1024);
                            outStream.Write(buffer, 0, bytesRead);
                        } while (bytesRead > 0);
                        return outStream.ToArray();
                    }
                }
            }
        }
        
        private static bool VerifyBytes3des(byte[] key, byte[] source)
        {
            var err = true;
            using (var hmac = new MACTripleDES(key))
            {
                var storedHash = new byte[hmac.HashSize / 8];
                using (var inStream = new MemoryStream(source))
                {
                    inStream.Read(storedHash, 0, storedHash.Length);
                    var computedHash = hmac.ComputeHash(inStream);
                    for (int i = 0; i < storedHash.Length; i++)
                    {
                        if (computedHash[i] != storedHash[i])
                        {
                            err = false;
                        }
                    }
                }
            }
            return err;
        }

        private static byte[] SignBytesHmacMd5(byte[] key, byte[] source)
        {
            using (var hmac = new HMACMD5(key))
            {
                using (var inStream = new MemoryStream(source))
                {
                    using (var outStream = new MemoryStream())
                    {
                        var  hashValue = hmac.ComputeHash(inStream);
                        inStream.Position = 0;
                        outStream.Write(hashValue, 0, hashValue.Length);
                        int bytesRead;
                        var buffer = new byte[1024];
                        do
                        {
                            bytesRead = inStream.Read(buffer, 0, 1024);
                            outStream.Write(buffer, 0, bytesRead);
                        } while (bytesRead > 0);
                        return outStream.ToArray();
                    }
                }
            }
        }

        private static bool VerifyBytesHmac(byte[] key, byte[] source)
        {
            var err = true;
            using (var hmac = new HMACMD5(key))
            {
                var storedHash = new byte[hmac.HashSize / 8];
                using (var inStream = new MemoryStream(source))
                {
                    inStream.Read(storedHash, 0, storedHash.Length);
                    var computedHash = hmac.ComputeHash(inStream);

                    for (int i = 0; i < storedHash.Length; i++)
                    {
                        if (computedHash[i] != storedHash[i])
                        {
                            err = false;
                        }
                    }
                }
            }
            return err;

        }
    }
}
