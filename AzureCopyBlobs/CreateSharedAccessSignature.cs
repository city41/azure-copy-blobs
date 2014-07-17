using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.IO;
using Microsoft.WindowsAzure;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;

namespace AzureCopyBlobs {
    static class CreateSharedAccessSignature {
        private static string GetBlobSasUri(CloudBlobContainer container, string blobName) {
            CloudBlockBlob blob = container.GetBlockBlobReference(blobName);

            //Set the expiry time and permissions for the blob.
            SharedAccessBlobPolicy sasConstraints = new SharedAccessBlobPolicy();
            sasConstraints.SharedAccessStartTime = DateTime.UtcNow.AddMinutes(-5);
            sasConstraints.SharedAccessExpiryTime = DateTime.UtcNow.AddMinutes(Config.SASDurationInMinutes);
            sasConstraints.Permissions = SharedAccessBlobPermissions.Read | SharedAccessBlobPermissions.List;

            string sasBlobToken = blob.GetSharedAccessSignature(sasConstraints);

            return blob.Uri + sasBlobToken;
        }

        public static Uri GetSasUri(string keyName, string containerName, string blobName) {
            return new Uri(GetSasString(keyName, containerName, blobName));
        }

        public static string GetSasString(string keyName, string containerName, string blobName) {
            CloudBlobClient blobClient = CloudAccount.GetClient(keyName);

            CloudBlobContainer container = blobClient.GetContainerReference(containerName);
            if (!container.Exists()) {
                throw new Exception(string.Format("Container {0} does not exist", containerName));
            }

            return GetBlobSasUri(container, blobName);
        }
    }
}
