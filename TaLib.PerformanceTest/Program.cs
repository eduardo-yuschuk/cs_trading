/*
   Copyright 2014 Eduardo Yuschuk (eduardo.yuschuk@gmail.com)
*/

using DukascopyQuote.FinancialDataProvider;
using FinancialData.Shared;
using System;
using System.Collections.Generic;
using TaLib.Extension;

namespace TaLib.PerformanceTest
{
    class Program
    {
        static void Main(string[] args)
        {
            Program program = new Program();
            program.TestSMA();
        }

        public void TestSMA()
        {
            List<IQuote> prices = new List<IQuote>();
            float[] dprices = new float[100];
            int dpricesIx = 0;
            Asset asset = new Asset("EUR/USD", AssetType.Currency);
            IDataSource source = new DukascopyDataSource();
            DukascopyOfflineProvider.Instance.AsyncGetHistory(
                asset,
                new DateTime(2006, 1, 1),
                new DateTime(2007, 1, 1),
                new DukascopyOfflineContext(@"C:\Users\user\EURUSD_Ticks_2010.01.01_2017.12.19.csv", SampleType.Quote,
                    TimeSpan.Zero),
                (Func<ISample, bool>) (sample =>
                {
                    prices.Add((IQuote) sample);
                    dprices[dpricesIx++] = (float) ((IQuote) sample).Ask;
                    return prices.Count < 100;
                }));
            // parallel
#if _
      Parallel.For(0, 10, (i) => {
        DateTime begin = DateTime.Now;
        for (int iterationIx = 0; iterationIx < 5000000; iterationIx++) {
          var smaValues = prices.SMA(14);
        }
        Console.WriteLine("Iteration time {0}: {1}", i, DateTime.Now - begin);
      });
#endif
            // quote based
#if _
      for (int testIx = 0; testIx < 10; testIx++) {
        DateTime begin = DateTime.Now;
        for (int iterationIx = 0; iterationIx < 5000000; iterationIx++) {
          var smaValues = prices.SMA(14);
        }
        Console.WriteLine("Itration time: {0}", DateTime.Now - begin);
      }
#endif
            // float array based
#if _
      for (int testIx = 0; testIx < 10; testIx++) {
        DateTime begin = DateTime.Now;
        for (int iterationIx = 0; iterationIx < 5000000; iterationIx++) {
          var smaValues = dprices.SMA(14);
        }
        Console.WriteLine("Itration time: {0}", DateTime.Now - begin);
      }
#endif
            // parallel, float array based
#if _
      DateTime begin = DateTime.Now;
      Parallel.For(0, 10, (i) => {
        for (int iterationIx = 0; iterationIx < 5000000; iterationIx++) {
          if (i % 2 == 0) {
            var smaValues = dprices.SMA(14);
          } else {
            var rsiValues = dprices.RSI(14);
          }
        }
      });
#endif
            // parallel, float array based
            DateTime begin = DateTime.Now;
            //Parallel.For(0, 5000000, (iterationIx) => {
            for (int iterationIx = 0; iterationIx < 5000000; iterationIx++)
            {
                if (iterationIx % 2 == 0)
                {
                    var smaValues = dprices.SMA(14);
                }
                else
                {
                    var rsiValues = dprices.RSI(14);
                }
            }

            //});
            Console.WriteLine("Iteration time {0}", DateTime.Now - begin);
            /////////////////////////////////////////////////////////////////////////
            Console.ReadKey(true);
        }
    }
}