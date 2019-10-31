using Amazon;
using Amazon.S3;
using System;
using System.IO;

namespace AmazonS3Upload
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            IAmazonS3 client = new AmazonS3Client("AKIA6PYYJMASGIKCXL53", "jK56S8EvgOA9lLNN8R3nNldE1IW2yVPXn9K0rNYH", RegionEndpoint.APSoutheast1);
            FileInfo file = new FileInfo(@"c:\test.txt");
            string destPath = "data/test.txt";
            S3FileInfo s3File = new S3FileInfo(client, "my-bucket-name", destPath);
        }
    }
}
