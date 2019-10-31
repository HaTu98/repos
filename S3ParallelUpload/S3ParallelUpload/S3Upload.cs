using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace S3ParallelUpload
{
    class S3Upload
    {
        private const string bucketName = "tuhvupload";
        private const string keyName = "file1";
        private const string filePath = @"C:\Users\TuHV\Documents\test1.mp4";
        static IAmazonS3 s3Client;
    }
}
