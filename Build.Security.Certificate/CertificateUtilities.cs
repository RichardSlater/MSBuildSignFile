using System;
using System.Security.Cryptography.X509Certificates;

namespace Build.Security.Certificate
{
    public class CertificateUtilities
    {
        public static X509Certificate2 GetCertificate(StoreName storeName, StoreLocation storeLocation, string thumbprint)
        {
            var certificateStore = new X509Store(storeName, storeLocation);
            try
            {
                certificateStore.Open(OpenFlags.ReadOnly);

                var certificates = certificateStore.Certificates.Find(X509FindType.FindByThumbprint, thumbprint, false);
                if (certificates.Count == 1)
                {
                    return certificates[0];
                }

                if (certificates.Count > 1)
                {
                    const string format = "{0} matching the thumbprint {1} were found.";
                    var message = String.Format(format, certificates.Count, thumbprint);
                    throw new InvalidOperationException(message);
                }
            }
            finally
            {
                certificateStore.Close();
            }

            return null;
        }
    }
}