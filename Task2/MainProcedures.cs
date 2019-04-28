using System;
using System.Security.Cryptography.X509Certificates;
using System.Security.Cryptography;
using System.Net;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using HashLib;
using System.Text;

namespace Task2
{
    public static class MainProcedures
    {

        internal static int RunSign(Options.SignatureOptions opts)
        {
            try
            {
                var content = File.ReadAllBytes(opts.FilePath);
                var hashProvider = HashFactory.Crypto.CreateMD4();
                var hash = hashProvider.ComputeBytes(content);

                var collection = new X509Certificate2Collection();
                collection.Import(opts.CertificatePath, opts.Password, X509KeyStorageFlags.PersistKeySet);
                var cert = collection[0];
                using (var rsa = cert.GetRSAPublicKey())
                {
                    var encryptedHash = rsa.Encrypt(hash.GetBytes(), RSAEncryptionPadding.OaepSHA1);
                    File.WriteAllBytes(opts.Output, encryptedHash);
                }
            }
            catch (Exception e)
            {
                return -1;
            }
            return 0;
        }

        internal static int RunVerify(Options.VerifyOptions opts)
        {
            try
            {
                var content = File.ReadAllBytes(opts.FilePath);
                var hashProvider = HashFactory.Crypto.CreateMD4();
                var actualHash = Convert.ToBase64String(hashProvider.ComputeBytes(content).GetBytes());
                var collection = new X509Certificate2Collection();
                collection.Import(opts.CertificatePath, opts.Password, X509KeyStorageFlags.PersistKeySet);
                var cert = collection[0];
                using (var rsa = cert.GetRSAPrivateKey())
                {
                    var signature = File.ReadAllBytes(opts.Signature);
                    var decryptedHash = Convert.ToBase64String(rsa.Decrypt(signature, RSAEncryptionPadding.OaepSHA1));
                    Console.WriteLine($"Content: {Encoding.UTF8.GetString(content)}");
                    Console.WriteLine($"Provided Hash: {decryptedHash}");
                    Console.WriteLine($"Actual Hash: {actualHash}");
                    Console.WriteLine($"Equality: {actualHash == decryptedHash}");
                    content[5] = 0x9;
                    Console.WriteLine($"Content: {Encoding.UTF8.GetString(content)}");
                    Console.WriteLine($"Provided Hash: {decryptedHash}");
                    actualHash = Convert.ToBase64String(hashProvider.ComputeBytes(content).GetBytes());
                    Console.WriteLine($"Actual Hash: {actualHash}");
                    Console.WriteLine($"Equality: {actualHash == decryptedHash}");
                }
            }
            catch (Exception e)
            {
                return -1;
            }
            return 0;
        }
    }
}
