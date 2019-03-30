using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PhoneServiceClientPrototype
{
    class Program
    {
        static void Main(string[] args)
        {
            PhoneHighScoreService.PhoneHighScoreServiceClient phssc = new PhoneHighScoreService.PhoneHighScoreServiceClient();
            Console.WriteLine(phssc.GetHighScoresForPlatform_Phone(PhoneHighScoreService.Platform.WindowsPhone, "WhaleRider"));
            Console.WriteLine(phssc.GetHighScoresGlobally_Phone("WhaleRider"));
            Console.ReadLine();
        }
    }
}
