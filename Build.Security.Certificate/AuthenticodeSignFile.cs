using System;
using Microsoft.Build.Utilities;
using System.Security.Cryptography.X509Certificates;
using Microsoft.Build.Tasks.Deployment.ManifestUtilities;
using Microsoft.Build.Framework;

namespace Build.Security.Certificate
{
    public class AuthenticodeSignFile : Task
    {
        private StoreLocation _storeLocation = StoreLocation.CurrentUser;
        private StoreName _storeName = StoreName.My;

        public string CertificateStoreName
        {
            get { return _storeName.ToString(); }
            set
            {
                _storeName = (StoreName)Enum.Parse(typeof(StoreName), value);
            }
        }

        public string CertificateStoreLocation
        {
            get { return _storeLocation.ToString(); }
            set
            {
                _storeLocation = (StoreLocation)Enum.Parse(typeof(StoreLocation), value);
            }
        }

        [Required]
        public string Thumbprint { get; set; }

        [Required]
        public string TimestampUrl { get; set; }

        [Required]
        public string FilePath { get; set; }

        public override bool Execute()
        {
            var timestampUrl = GetTimestampUrl();
            var certificate = CertificateUtilities.GetCertificate(_storeName, _storeLocation, Thumbprint);

            if (certificate == null)
            {
                Log.LogWarning("Certificate wtih thumbprint {0} not found in the {1}\\{2} store.", Thumbprint, _storeLocation, _storeName);
                return false;
            }

            SecurityUtilities.SignFile(certificate, timestampUrl, FilePath);

            Log.LogMessage("Successfully signed {0} with certificate ({1}) from {2}\\{3}.", FilePath, Thumbprint, _storeLocation, _storeName);

            return true;
        }

        private Uri GetTimestampUrl()
        {
            Uri timestampUrl;

            if (String.IsNullOrEmpty(TimestampUrl) || !Uri.TryCreate(TimestampUrl, UriKind.Absolute, out timestampUrl))
            {
                Log.LogWarning("The timestamp URL ({0}) is empty or not valid.", TimestampUrl);
                return null;
            }

            return timestampUrl;
        }
    }
}
