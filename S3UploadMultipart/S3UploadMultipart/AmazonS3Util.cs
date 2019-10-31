using Amazon.S3;
using Amazon.S3.Model;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;

namespace S3UploadMultipart
{
    class AmazonS3Util
    {

        private const string bucketName = "tuhvupload";
        private const string keyName = "file1";
        private const string filePath = @"C:\Users\TuHV\Documents\test1.mp4";
        public static void putObjectAsMultiPart()
        {
            Stopwatch sw = Stopwatch.StartNew();
            var s3Client = new AmazonS3Client(
                "AKIA6PYYJMASLJEFTI6E",
                "IJPo9Ys58iAb35dKw4kcW/SkOU2J+iI9IOA5Wpl6", 
                Amazon.RegionEndpoint.APSoutheast1
            );

            // List to store upload part responses.
            var uploadResponses = new List<UploadPartResponse>();

            // 1. Initialize.
            var initiateRequest = new InitiateMultipartUploadRequest
            {
                BucketName = bucketName,
                Key = keyName
            };

            var initResponse = s3Client.InitiateMultipartUpload(initiateRequest);

            // 2. Upload Parts.
            var contentLength = new FileInfo(filePath).Length;
            var partSize = 5242880; // 5 MB

            try
            {
                long filePosition = 0;
                for (var i = 1; filePosition < contentLength; ++i)
                {
                    // Create request to upload a part.
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
