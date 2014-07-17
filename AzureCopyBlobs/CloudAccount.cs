using Microsoft.WindowsAzure;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AzureCopyBlobs {
    static class CloudAccount {
        public static CloudBlobClient GetClient(string accountConnectionString) {
            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(accountConnectionString);
            return storageAccount.CreateCloudBlobClient();
        }
    }
}
