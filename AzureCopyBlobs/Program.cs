using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace AzureCopyBlobs
{
    class Program
    {
        static void CopyBlob(string blobName) {
            string result = CopyBlobAcross.CopyBlob(
                Config.SourceStorageAccountConnectionString, 
                Config.DestStorageAccountConnectionString,
                Config.ContainerName, 
                blobName, 
                true
            );

            Console.WriteLine(result);
        }

        static void CopyBlobs() {
            foreach(var blobName in Config.BlobsToCopy) {
                Console.WriteLine("About to copy {0}", blobName);
                CopyBlob(blobName);
            }
        }

        static void Main(string[] args)
        {
            CopyBlobs();
            Console.WriteLine("Press a key to continue");
            Console.ReadKey();
        }
    }
}
