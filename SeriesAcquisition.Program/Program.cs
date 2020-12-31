/*
   Copyright 2014 Eduardo Yuschuk (eduardo.yuschuk@gmail.com)
*/

using System;

namespace SeriesAcquisition.Program
{
    class Program
    {
        static void Main(string[] args)
        {
            DateTime begin = new DateTime(2012, 1, 1);
            DateTime end = new DateTime(2012, 2, 1);
            SeriesAcquisitor.RequestTicks("EUR/USD", begin, end, @"C:\quotes\EURUSD\Dukascopy\");
            Console.WriteLine("Press any key to exit...");
            Console.ReadKey(true);
        }
    }
}