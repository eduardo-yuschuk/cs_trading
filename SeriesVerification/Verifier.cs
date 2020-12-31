/*
   Copyright 2014 Eduardo Yuschuk (eduardo.yuschuk@gmail.com)
*/

using System;
using QuotesConversion;
using System.IO;
using System.Threading;

namespace SeriesVerification
{
    public class Verifier
    {
        public static bool VerifySequentiality(string filePath)
        {
            DateTime previousDateTime = DateTime.MinValue;
            DateTime dateTime;
            for (int i = 0; i < 10; i++)
            {
                try
                {
                    using (StreamReader reader = File.OpenText(filePath))
                    {
                        while (QuotesConverter.GetQuoteDateTimeFromStreamReader(reader, out dateTime))
                        {
                            if (dateTime < previousDateTime)
                            {
                                return false;
                            }
                        }
                    }
                }
                catch (IOException ioe)
                {
                    Console.WriteLine(ioe.Message);
                    Thread.Sleep(1000);
                }
            }

            return true;
        }
    }
}