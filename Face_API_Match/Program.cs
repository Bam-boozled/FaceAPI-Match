using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Face_API_Match
{
    class Program
    {
        static void Main(string[] args)
        {
            string imageFilePath = "C:/Users/c.dams/Downloads/face_test_unknown_1.jpg";

        
            Groups.InsertGroupID();
            Person.PutPersonsInlist().Wait();
            var watch = System.Diagnostics.Stopwatch.StartNew();
            HoloFoto.HoloFaceId(imageFilePath).Wait();
            Match.ShowBestMatch().Wait();
            watch.Stop();
            var elapsedMs = watch.ElapsedMilliseconds;
            Console.WriteLine();
            Console.WriteLine(elapsedMs);
            Console.ReadLine();
        }
    }
}
