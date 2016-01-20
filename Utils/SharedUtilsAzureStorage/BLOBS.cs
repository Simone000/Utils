using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;

namespace SharedUtilsAzureStorage
{
    public static class BLOBS
    {
        /// <summary>
        /// using(var memoryStream = new MemoryStream())
        /// {
        ///     await DownloadBlobToStreamAsync(memoryStream, blobname, containername, connectionstring);
        /// }
        /// </summary>
        /// <param name="FileStream"></param>
        /// <param name="BlobName"></param>
        /// <param name="ContainerName"></param>
        /// <param name="StorageConnectionString">DefaultEndpointsProtocol=https;AccountName=...;AccountKey=...</param>
        /// <returns></returns>
        public static async Task DownloadBlobToStreamAsync(Stream FileStream,
                                                           string BlobName,
                                                           string ContainerName,
                                                           string StorageConnectionString)
        {
            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(StorageConnectionString);
            CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();
            CloudBlobContainer container = blobClient.GetContainerReference(ContainerName);

            CloudBlockBlob blockBlob = container.GetBlockBlobReference(BlobName);
            await blockBlob.DownloadToStreamAsync(FileStream).ConfigureAwait(false);

            //riavvolgo lo stream per utilizzarlo
            FileStream.Seek(0, SeekOrigin.Begin);
        }

        /// <summary>
        /// Remember to dispose the stream!
        /// </summary>
        /// <param name="BlobName"></param>
        /// <param name="ContainerName"></param>
        /// <param name="StorageConnectionString"></param>
        /// <returns></returns>
        public static async Task<Stream> DownloadBlobAsStream(string BlobName,
                                                              string ContainerName,
                                                              string StorageConnectionString)
        {
            var stream = new MemoryStream();

            try
            {
                await DownloadBlobToStreamAsync(stream, BlobName, ContainerName, StorageConnectionString).ConfigureAwait(false);
            }
            catch(Exception Exc)
            {
                stream.Dispose();
                throw Exc;
            }
            return stream;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="FileStream">await File.ReadAsStreamAsync()</param>
        /// <param name="BlobName"></param>
        /// <param name="ContainerName"></param>
        /// <param name="StorageConnectionString">DefaultEndpointsProtocol=https;AccountName=...;AccountKey=...</param>
        /// <returns></returns>
        public static async Task UploadBlobAsync(Stream FileStream,
                                            string BlobName,
                                            string ContainerName,
                                            string StorageConnectionString,
                                            bool IsContainerPublic = false)
        {
            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(StorageConnectionString);
            CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();
            CloudBlobContainer container = blobClient.GetContainerReference(ContainerName);

            var created = await container.CreateIfNotExistsAsync().ConfigureAwait(false);
            if (created)
            {
                if(IsContainerPublic)
                {
                    throw new NotImplementedException("Implementato solo per container privato");
                }

                await container.SetPermissionsAsync(new BlobContainerPermissions
                {
                    PublicAccess = BlobContainerPublicAccessType.Off
                }).ConfigureAwait(false);
            }

            CloudBlockBlob blockBlob = container.GetBlockBlobReference(BlobName);
            await blockBlob.UploadFromStreamAsync(FileStream).ConfigureAwait(false);
        }


    }
}
