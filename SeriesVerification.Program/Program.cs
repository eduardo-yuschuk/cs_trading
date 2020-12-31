/*
   Copyright 2014 Eduardo Yuschuk (eduardo.yuschuk@gmail.com)
*/

using System;

namespace SeriesVerification.Program
{
    class Program
    {
        static void Main(string[] args)
        {
            if (!Verifier.VerifySequentiality(@"C:\Users\user\EURUSD_Ticks_2010.01.01_2017.12.19.csv"))
            {
                Console.WriteLine("Error on text file.");
            }
            else
            {
                Console.WriteLine("The text file is fine.");
            }

            Console.WriteLine("Press any key to exit...");
            Console.ReadKey(true);
        }
    }
}