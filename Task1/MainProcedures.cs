using System;
using System.Security.Cryptography.X509Certificates;
using static Task1.Options;
using System.Security.Cryptography;
using System.Net;
using System.IO;

namespace Task1
{
    public static class MainProcedures
    {
        public static int RunMakeSelfCert(MakeSelfCertOptions opts)
        {
            try
            {
                var cert = BuildSelfSignedServerCertificate(opts.Password);
                File.WriteAllBytes("./selfCert.pfx", cert);
            }
            catch (Exception e)
            {
                return -1;
            }
            return 0;
        }

        public static int RunMakeCert(MakeCertOptions opts)
        {
            try
            {
                var collection = new X509Certificate2Collection();
                collection.Import(opts.CertificatePath, opts.Password, X509KeyStorageFlags.PersistKeySet);
                var issuer = collection[0];
                var cert = BuildSignedServerCertificate(opts.Password, issuer);
                File.WriteAllBytes("./cert.pfx", cert);
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
            }
            catch (Exception e)
            {
                return -1;
            }
            return 0;
        }

        private static byte[] BuildSelfSignedServerCertificate(string password)
        {
            SubjectAlternativeNameBuilder sanBuilder = new SubjectAlternativeNameBuilder();
            sanBuilder.AddIpAddress(IPAddress.Loopback);
            sanBuilder.AddIpAddress(IPAddress.IPv6Loopback);
            sanBuilder.AddDnsName("localhost");
            sanBuilder.AddDnsName(Environment.MachineName);

            X500DistinguishedName distinguishedName = new X500DistinguishedName($"CN=SSNCert1");

            using (RSA rsa = RSA.Create(2048))
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

        private static byte[] BuildSignedServerCertificate(string password, X509Certificate2 issuer)
        {
            SubjectAlternativeNameBuilder sanBuilder = new SubjectAlternativeNameBuilder();
            sanBuilder.AddIpAddress(IPAddress.Loopback);
            sanBuilder.AddIpAddress(IPAddress.IPv6Loopback);
            sanBuilder.AddDnsName("localhost");
            sanBuilder.AddDnsName(Environment.MachineName);

            X500DistinguishedName distinguishedName = new X500DistinguishedName($"CN=SSNCert2");

            using (RSA rsa = RSA.Create(2048))
            {
                var request = new CertificateRequest(distinguishedName, rsa, HashAlgorithmName.SHA256, RSASignaturePadding.Pkcs1);

                request.CertificateExtensions.Add(
                    new X509KeyUsageExtension(X509KeyUsageFlags.DataEncipherment | X509KeyUsageFlags.KeyEncipherment, false));
                request.CertificateExtensions.Add(
                   new X509EnhancedKeyUsageExtension(
                       new OidCollection { new Oid("1.3.6.1.5.5.7.3.1") }, false));

                request.CertificateExtensions.Add(sanBuilder.Build());
                var serial = 42;
                var certificate = request.Create(issuer, new DateTimeOffset(DateTime.UtcNow), new DateTimeOffset(DateTime.UtcNow.AddDays(365)), BitConverter.GetBytes(serial));
                certificate.FriendlyName = "SSNCert1";

                return certificate.Export(X509ContentType.Pfx, password);
            }
        }
    }
}
