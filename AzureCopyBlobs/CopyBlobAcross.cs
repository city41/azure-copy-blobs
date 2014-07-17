using Microsoft.WindowsAzure.Storage.Blob;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace AzureCopyBlobs {
    static class CopyBlobAcross {
        private static void PrintStatus(ICloudBlob backupBlob) {
            Console.WriteLine();
            Console.WriteLine("\t{0} |> Status:{1} |> {2}",
                              DateTime.Now.ToString(),
                              backupBlob.CopyState.Status,
                              backupBlob.CopyState.StatusDescription);

            double bytesCopied = 0;
            if (backupBlob.CopyState.BytesCopied.HasValue)
                bytesCopied = backupBlob.CopyState.BytesCopied.Value;

            var totalBytes = backupBlob.CopyState.TotalBytes;

            if (totalBytes.HasValue) {
                Console.WriteLine("\t{0}/{1} bytes copied",
                                  (bytesCopied)
                                  .ToString(CultureInfo.InvariantCulture),
                                  totalBytes.Value.ToString());
                Console.WriteLine("\tProgress : {0} %",
                                  (bytesCopied / totalBytes.Value) * 100);
            }

            Console.WriteLine();
        }

        private static void PrintTaskProgress(CloudBlobContainer container, string blobName) {
            ICloudBlob blob = container.GetBlobReferenceFromServer(blobName);

            while (blob.CopyState.Status != CopyStatus.Success && blob.CopyState.Status != CopyStatus.Failed) {
                PrintStatus(blob);
                Task.Delay(TimeSpan.FromSeconds(20d)).Wait();
                blob = container.GetBlobReferenceFromServer(blobName);
            }

            PrintStatus(blob);
        }

        private static ICloudBlob GetCloudBlob(CloudBlobClient client, string containerName, string blobName, bool createContainerIfNotExists, out CloudBlobContainer container) {
            container = client.GetContainerReference(containerName);

            if (createContainerIfNotExists) {
                container.CreateIfNotExists();
            }

            if (!container.Exists()) {
                throw new Exception(string.Format("Container {0} does not exist", containerName));
            }

            return container.GetPageBlobReference(blobName);
        }

        static void CopyBlobTo(string srcAccount, string destAccount, string containerName, string blobName, bool createDest) {
            var now = DateTime.Now;

            var sourceBlobUri = CreateSharedAccessSignature.GetSasUri(srcAccount, containerName, blobName);
            var destClient = CloudAccount.GetClient(destAccount);
            CloudBlobContainer destContainer = null;
            var destinationBlob = GetCloudBlob(destClient, containerName, blobName, createDest, out destContainer);

            var taskId = destinationBlob.StartCopyFromBlob(sourceBlobUri);

            PrintTaskProgress(destContainer, blobName);
        }


        public static string CopyBlob(string srcAccount, string destAccount, string containerName, string blobName, bool createDest) {
            CopyBlobTo(srcAccount, destAccount, containerName, blobName, createDest);

            return string.Format("Copied {0}/{1} successfully", containerName, blobName);
        }
    }
}
