using System;
using System.Security.Cryptography.X509Certificates;
using static Task1.Options;
using System.Security.Cryptography;
using System.Net;
using System.IO;
using System.Collections.Generic;
using System.Linq;

namespace Task1
{
    public static class MainProcedures
    {
        public static int RunMakeSelfCert(MakeSelfCertOptions opts)
        {
            try
            {
                var cert = BuildSelfSignedCertificate(opts.Password);
                File.WriteAllBytes("./selfCert.pfx", cert);
            }
            catch (Exception e)
            {
                return -1;
            }
            return 0;
        }

        public static int RunEncrypt(EncryptOptions opts)
        {
            try
            {
                var collection = new X509Certificate2Collection();
                collection.Import(opts.CertificatePath, opts.Password, X509KeyStorageFlags.PersistKeySet);
                var cert = collection[0];
                var data = File.ReadAllText(opts.FilePath);

                var tdes = new TripleDESCryptoServiceProvider();
                tdes.GenerateKey();
                var encryptor = tdes.CreateEncryptor();
                byte[] encData;
                using (var msEncrypt = new MemoryStream())
                {
                    using (var csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                    {
                        using (var swEncrypt = new StreamWriter(csEncrypt))
                        {
                            swEncrypt.Write(data);
                        }
                        encData = msEncrypt.ToArray();
                    }
                }
                using (var rsa = cert.GetRSAPublicKey())
                {
                    var encryptedKey =  rsa.Encrypt(tdes.Key, RSAEncryptionPadding.OaepSHA1);
                    var encryptedIV = rsa.Encrypt(tdes.IV, RSAEncryptionPadding.OaepSHA1);
                    var dataTotal = encData.Concat(encryptedKey).Concat(encryptedIV).ToArray();
                    File.WriteAllBytes(opts.Output, dataTotal);
                }
            }
            catch (Exception e)
            {
                return -1;
            }
            return 0;
        }

        public static int RunDecrypt(DecryptOptions opts)
        {
            try
            {
                var collection = new X509Certificate2Collection();
                collection.Import(opts.CertificatePath, opts.Password, X509KeyStorageFlags.PersistKeySet);
                var cert = collection[0];
                var encryptedTotal = File.ReadAllBytes(opts.FilePath);
                var simmKeyIVEncrypted = encryptedTotal.Skip(Math.Max(0, encryptedTotal.Count() - 512)).ToArray();
                var simmKeyEncrypted = simmKeyIVEncrypted.Take(256).ToArray();
                var simmIVEncrypted = simmKeyIVEncrypted.Skip(256).ToArray();
                using (var rsa = cert.GetRSAPrivateKey())
                {
                    var decryptedKey = rsa.Decrypt(simmKeyEncrypted, RSAEncryptionPadding.OaepSHA1);
                    var decryptedIV = rsa.Decrypt(simmIVEncrypted, RSAEncryptionPadding.OaepSHA1);
                    var tdes = new TripleDESCryptoServiceProvider
                    {
                        Key = decryptedKey,
                        IV = decryptedIV
                    };
                    var decryptor = tdes.CreateDecryptor();
                    var payload = encryptedTotal.Take(encryptedTotal.Count() - 512).ToArray();
                    using (var msDecrypt = new MemoryStream(payload))
                    {
                        using (var csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                        {
                            using (var srDecrypt = new StreamReader(csDecrypt))
                            {
                                var res = srDecrypt.ReadToEnd();
                                File.WriteAllText(opts.Output, res);
                            }
                        }
                    }
                }
            }
            catch (Exception e)
            {
                return -1;
            }
            return 0;
        }

        private static byte[] BuildSelfSignedCertificate(string password)
        {
            var sanBuilder = new SubjectAlternativeNameBuilder();
            sanBuilder.AddIpAddress(IPAddress.Loopback);
            sanBuilder.AddIpAddress(IPAddress.IPv6Loopback);
            sanBuilder.AddDnsName("localhost");
            sanBuilder.AddDnsName(Environment.MachineName);

            var distinguishedName = new X500DistinguishedName($"CN=SSNCert1");

            using (var rsa = RSA.Create(2048))
            {
                var request = new CertificateRequest(distinguishedName, rsa, HashAlgorithmName.SHA256, RSASignaturePadding.Pkcs1);

                request.CertificateExtensions.Add(
                    new X509KeyUsageExtension(X509KeyUsageFlags.DataEncipherment | X509KeyUsageFlags.KeyEncipherment | X509KeyUsageFlags.DigitalSignature | X509KeyUsageFlags.KeyCertSign, false));
                
                request.CertificateExtensions.Add(
                   new X509EnhancedKeyUsageExtension(
                       new OidCollection { new Oid("1.3.6.1.5.5.7.3.1") }, false));
                request.CertificateExtensions.Add(new X509BasicConstraintsExtension(true, false, 0, true));
                request.CertificateExtensions.Add(sanBuilder.Build());

                var certificate = request.CreateSelfSigned(new DateTimeOffset(DateTime.UtcNow.AddDays(-1)), new DateTimeOffset(DateTime.UtcNow.AddDays(3650)));
                certificate.FriendlyName = "SSNCert1";

                return certificate.Export(X509ContentType.Pfx, password);
            }
        }
    }
}
