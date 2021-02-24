using System;

namespace Business.CCS
{
    public class DatabasseLogger : ILogger
    {
        public void Log()
        {
            Console.WriteLine("Veritabanına loglandı.");
        }
    }
}
