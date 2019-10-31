using Amazon.S3;
using Amazon.S3.Model;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace Amazons3DotNetFramework
{
    class AmazonS3Util
    {

        private const string bucketName = "tuhvupload";
        private const string keyName = "file1";
        private const string filePath = @"C:\Users\TuHV\Documents\test1.mp4";
        static IAmazonS3 s3Client;
        public static async Task PutObjectAsMultiPartAsync()
        {
            Stopwatch sw = Stopwatch.StartNew();
            s3Client = new AmazonS3Client(
                "AKIA6PYYJMASLJEFTI6E",
                "IJPo9Ys58iAb35dKw4kcW/SkOU2J+iI9IOA5Wpl6",
                Amazon.RegionEndpoint.APSoutheast1
            );
            List<UploadPartResponse> uploadResponses = new List<UploadPartResponse>();
            // List to store upload part responses.


            // 1. Initialize.
            var initiateRequest = new InitiateMultipartUploadRequest
            {
                BucketName = bucketName,
                Key = keyName
            };

            var initResponse = s3Client.InitiateMultipartUpload(initiateRequest);

            // 2. Upload Parts.
            var partNumber = 5;
            var contentLength = new FileInfo(filePath).Length;
            
            var partSize = contentLength/partNumber;
            long filePosition = 0;
            try
            {
                for (var i = 1; filePosition < contentLength; ++i)
                {
                    // Create request to upload a part.
                    Console.WriteLine(filePosition);
                    var uploadRequest = new UploadPartRequest
                    {
                        BucketName = bucketName,
                        Key = keyName,
                        UploadId = initResponse.UploadId,
                        PartNumber = i,
                        PartSize = partSize,
                        FilePosition = filePosition,
                        FilePath = filePath
                    };

                    // Upload part and add response to our list.
                    uploadResponses.Add(s3Client.UploadPart(uploadRequest));

                    filePosition += partSize;
                }

                // Step 3: complete.
                var completeRequest = new CompleteMultipartUploadRequest
                {
                    BucketName = bucketName,
                    Key = keyName,
                    UploadId = initResponse.UploadId,
                };

                // add ETags for uploaded files
                completeRequest.AddPartETags(uploadResponses);

                var completeUploadResponse = s3Client.CompleteMultipartUpload(completeRequest);
                sw.Stop();
                Console.WriteLine("Upload completed : {0}", sw.Elapsed.TotalMilliseconds);
            }
            catch (Exception exception)
            {
                Console.WriteLine("Exception occurred: {0}", exception.ToString());
                var abortMPURequest = new AbortMultipartUploadRequest
                {
                    BucketName = bucketName,
                    Key = keyName,
                    UploadId = initResponse.UploadId
                };
                s3Client.AbortMultipartUpload(abortMPURequest);
            }
        }
    }

    


}
