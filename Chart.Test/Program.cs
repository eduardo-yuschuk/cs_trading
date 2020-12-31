/*
   Copyright 2014 Eduardo Yuschuk (eduardo.yuschuk@gmail.com)
*/

using Charts;
using Charts.Common;
using Simulation.Shared;
using System;
using System.Collections.Generic;
using System.Windows.Media;

namespace Chart.Test
{
    class Program
    {
        static void Main(string[] args)
        {
            ChartPool.CreateChart();

            var prices = CreateRandomLine("Prices", new DateTime(2009, 6, 1), new DateTime(2009, 6, 15));
            ChartPool.AddSeries(new List<Series> {prices});
            //Pause();

            //ChartPool.ClearSeries();
            //Pause();

            Random random = new Random(1);
            double rand() => random.Next((int) (prices.MinValue * 10000), (int) (prices.MaxValue * 10000)) / 10000.0;


            var trades = new Series(
                "Trades",
                ChartType.Trades,
                Colors.Black,
                new List<IDrawable>
                {
                    new Trade(new DateTime(2009, 6, 1), new DateTime(2009, 6, 7), PositionSide.Long, rand(), rand()),
                    new Trade(new DateTime(2009, 6, 7), new DateTime(2009, 6, 8), PositionSide.Long, rand(), rand()),
                    new Trade(new DateTime(2009, 6, 9), new DateTime(2009, 6, 12), PositionSide.Long, rand(), rand()),
                    new Trade(new DateTime(2009, 6, 11), new DateTime(2009, 6, 14), PositionSide.Long, rand(), rand()),
                });
            ChartPool.AddSeries(new List<Series> {trades});
            Pause();

            ChartPool.ClearSeries();
            Pause();
        }

        static Series CreateRandomLine(string name, DateTime begin, DateTime end)
        {
            Random random = new Random();
            List<Sample> samples = new List<Sample>();
            for (DateTime dt = begin; dt < end; dt += TimeSpan.FromDays(1))
            {
                samples.Add(new Sample(dt, random.NextDouble()));
            }

            return new Series(name, ChartType.DotsAndLines, Colors.Black, samples);
        }

        static void Pause()
        {
            Console.WriteLine("Press any key to exit...");
            Console.ReadKey(true);
        }
    }
}