using System;


namespace S3UploadMultipart
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            AmazonS3Util.putObjectAsMultiPart();
        }
    }

}
