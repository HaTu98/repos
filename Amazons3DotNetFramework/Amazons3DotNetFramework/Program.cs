using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Amazons3DotNetFramework
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("start");
            AmazonS3Util.PutObjectAsMultiPartAsync();
            Console.WriteLine("done");
            Console.ReadLine();
        }
    }
}
