/*
   Copyright 2014 Eduardo Yuschuk (eduardo.yuschuk@gmail.com)
*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Windows;
using System.Windows.Controls;
using Simulation.Shared;
using Charts.Common;

namespace Charts
{
    public class Series
    {
        public bool IsRenderable => Name != null;
        public bool IsPrimary { get; set; }
        public string Name { get; set; }
        private readonly Color _color;
        public Brush Brush => IsPrimary ? Primarybrush : new SolidColorBrush(_color);
        private static readonly Brush Primarybrush = Brushes.BlueViolet;
        private static readonly Color UndefinedColor = Colors.Black;
        public List<IDrawable> Drawables { get; set; }

        public Series(string name, ChartType chartType, Color color, IEnumerable<IDrawable> drawables)
        {
            Name = name;
            Drawables = new List<IDrawable>(drawables);
            ChartType = chartType;
            _color = color;
        }

        public Series(string name, ChartType chartType, Color color)
            : this(name, chartType, color, new List<IDrawable>())
        {
        }

        public Series(string name, ChartType chartType)
            : this(name, chartType, UndefinedColor, new List<IDrawable>())
        {
        }

        public Series Clone()
        {
            return new Series(Name, ChartType) { IsPrimary = IsPrimary };
        }

        public void SetDrawables(IEnumerable<IDrawable> drawables)
        {
            Drawables = new List<IDrawable>(drawables);
        }

        public static FrameworkElement GetRenderingLine(Series series, double actualWidth, double actualHeight,
            DateTime begin, DateTime end, double min, double max, bool drawPoints)
        {
            var samples = series.Drawables.Cast<Sample>().ToList();
            var grid = new Grid();
            var line = new Polyline
            {
                ToolTip = series.Name,
                Points = new PointCollection(),
                Stroke = series.Brush,
                StrokeThickness = series.IsPrimary ? 4 : 1
            };
            grid.Children.Add(line);
            var deltatimePeriod = (end - begin).TotalMilliseconds;
            var deltaMaxMin = max - min;
            samples.ForEach(sample =>
            {
                var deltatime = (sample.Instant - begin).TotalMilliseconds;
                if (sample.Instant < begin || sample.Instant > end) return;
                var point = new Point { X = deltatime * actualWidth / deltatimePeriod };
                var deltaValue = sample.Value - min;
                point.Y = deltaValue * actualHeight / deltaMaxMin;
                point.Y = actualHeight - point.Y; // inversion
                line.Points.Add(point);
                if (!drawPoints) return;
                var ellipse = new Ellipse
                {
                    ToolTip = $"{series.Name}: {sample.Instant} {sample.Value:F4}"
                };
                const double ellipseDiameter = 5;
                ellipse.HorizontalAlignment = HorizontalAlignment.Left;
                ellipse.VerticalAlignment = VerticalAlignment.Top;
                ellipse.Margin = new Thickness(point.X - (ellipseDiameter / 2.0), point.Y - (ellipseDiameter / 2.0), 0,
                    0);
                ellipse.Stroke = Brushes.Black;
                ellipse.Fill = Brushes.White;
                ellipse.Width = ellipse.Height = ellipseDiameter;
                grid.Children.Add(ellipse);
            });
            return grid;
        }

        public static FrameworkElement GetRenderingColumns(Series series, double actualWidth, double actualHeight,
            DateTime begin, DateTime end, double min, double max)
        {
            Console.WriteLine(@"actualWidth {0}, actualHeight {1}, begin {2}, end {3}, min {4}, max {5}",
                actualWidth, actualHeight, begin, end, min, max);
            //if (min > 0 || max < 0) {
            //  throw new InvalidDataException("GetRenderingColumns: min must be smaller than zero and max must be greater than zero.");
            //}
            var samples = series.Drawables.Cast<Sample>().ToList();
            var grid = new Grid();
            var deltatimePeriod = (end - begin).TotalMilliseconds;
            var deltaMaxMin = max - min;
            const double tolerance = 0;
            var relevantSamples = samples.Where(sample =>
                sample.Instant >= begin && sample.Instant <= end && Math.Abs(sample.Value) > tolerance).ToList();
            double reclangleWidth = 10; // actualWidth / relevantSamples.Count;
            if (relevantSamples.Count > 1)
            {
                var implicitInterval = relevantSamples[1].Instant - relevantSamples[0].Instant;
                reclangleWidth = implicitInterval.TotalMilliseconds * actualWidth / deltatimePeriod;
            }

            relevantSamples.ForEach(sample =>
            {
                var rectangle = new Rectangle
                {
                    HorizontalAlignment = HorizontalAlignment.Left,
                    VerticalAlignment = VerticalAlignment.Top,
                    Width = reclangleWidth
                };
                rectangle.Fill = rectangle.Stroke = sample.Value > 0 ? Brushes.Green : Brushes.Red;
                //rectangle.ToolTip = string.Format("{0}: {1} {2:F4}", series.Name, sample.Instant, sample.Value);
                var deltatime = (sample.Instant - begin).TotalMilliseconds;
                var xPosition = deltatime * actualWidth / deltatimePeriod;
                //---
                var deltaValue = sample.Value - min;
                var deltaZero = 0 - min;
                var zeroPosition = deltaZero * actualHeight / deltaMaxMin;
                double yPosition;
                if (sample.Value > 0)
                {
                    yPosition = actualHeight - (deltaValue * actualHeight / deltaMaxMin);
                }
                else
                {
                    yPosition = actualHeight - zeroPosition;
                }

                rectangle.Height = Math.Abs(sample.Value) * actualHeight / deltaMaxMin;
                if (rectangle.Height < 0.5) rectangle.Height = 0.5;

                //---
                rectangle.Margin = new Thickness(xPosition, yPosition, 0, 0);

                rectangle.ToolTip =
                    $"xPosition {xPosition:F1}, yPosition {yPosition:F1}, Width {rectangle.Width:F1}, Height {rectangle.Height:F1}, zeroPosition {zeroPosition:F1}";

                grid.Children.Add(rectangle);
            });
            return grid;
        }

        public static FrameworkElement GetRenderingTrades(Series series, double actualWidth, double actualHeight,
            DateTime begin, DateTime end, double min, double max)
        {
            var trades = series.Drawables.Cast<Trade>().ToList();
            var grid = new Grid();
            var deltatimePeriod = (end - begin).TotalMilliseconds;
            var deltaMaxMin = max - min;
            trades.ForEach(trade =>
            {
                if (trade.Begin >= begin && trade.End <= end)
                {
                    var line = new Line { StrokeThickness = 2 };
                    var sideColor = trade.Side == PositionSide.Long ? Brushes.Green : Brushes.Red;
                    line.Fill = line.Stroke = sideColor;
                    line.ToolTip =
                        $"Trade {trade.Side}  -  Duration {trade.End - trade.Begin}  -  Abs Delta {Math.Abs(trade.ClosePrice - trade.OpenPrice):F6}";
                    //position computation
                    var beginDeltatime = (trade.Begin - begin).TotalMilliseconds;
                    var beginXPosition = beginDeltatime * actualWidth / deltatimePeriod;
                    var endDeltatime = (trade.End - begin).TotalMilliseconds;
                    var endXPosition = endDeltatime * actualWidth / deltatimePeriod;
                    var beginDeltaValue = trade.OpenPrice - min;
                    var beginYPosition = beginDeltaValue * actualHeight / deltaMaxMin;
                    var endDeltaValue = trade.ClosePrice - min;
                    var endYPosition = endDeltaValue * actualHeight / deltaMaxMin;
                    beginYPosition = actualHeight - beginYPosition; // inversion
                    endYPosition = actualHeight - endYPosition; // inversion
                    //position setting
                    line.HorizontalAlignment = HorizontalAlignment.Left;
                    line.VerticalAlignment = VerticalAlignment.Top;
                    line.X1 = beginXPosition;
                    line.Y1 = beginYPosition;
                    line.X2 = endXPosition;
                    line.Y2 = endYPosition;
                    grid.Children.Add(line);
                    // open trade point
                    var ellipse = new Ellipse();
                    const double ellipseDiameter = 6;
                    ellipse.HorizontalAlignment = HorizontalAlignment.Left;
                    ellipse.VerticalAlignment = VerticalAlignment.Top;
                    ellipse.Margin = new Thickness(line.X1 - (ellipseDiameter / 2.0), line.Y1 - (ellipseDiameter / 2.0),
                        0, 0);
                    ellipse.Stroke = sideColor;
                    ellipse.Fill = sideColor;
                    ellipse.Width = ellipse.Height = ellipseDiameter;
                    ellipse.ToolTip = $"Open {trade.Begin}  -  {trade.OpenPrice:F6}";
                    grid.Children.Add(ellipse);
                    // close trade point
                    ellipse = new Ellipse();
                    ellipse.HorizontalAlignment = HorizontalAlignment.Left;
                    ellipse.VerticalAlignment = VerticalAlignment.Top;
                    ellipse.Margin = new Thickness(line.X2 - (ellipseDiameter / 2.0), line.Y2 - (ellipseDiameter / 2.0),
                        0, 0);
                    ellipse.Stroke = sideColor;
                    ellipse.Fill = sideColor;
                    ellipse.Width = ellipse.Height = ellipseDiameter;
                    ellipse.ToolTip = $"Open {trade.End}  -  {trade.ClosePrice:F6}";
                    grid.Children.Add(ellipse);
                }
            });
            return grid;
        }

        public static FrameworkElement GetRenderingCandles(Series series, double actualWidth, double actualHeight,
            DateTime begin, DateTime end, double min, double max)
        {
            Console.WriteLine(@"actualWidth {0}, actualHeight {1}, begin {2}, end {3}, min {4}, max {5}",
                actualWidth, actualHeight, begin, end, min, max);
            //if (min > 0 || max < 0) {
            //  throw new InvalidDataException("GetRenderingColumns: min must be smaller than zero and max must be greater than zero.");
            //}
            var samples = series.Drawables.Cast<Candle>().ToList();
            var grid = new Grid();
            var deltatimePeriod = (end - begin).TotalMilliseconds;
            var deltaMaxMin = max - min;
            const double tolerance = 0;
            var relevantSamples = samples.Where(sample =>
                sample.Begin >= begin && sample.Begin <= end && Math.Abs(sample.Close) > tolerance).ToList();
            var reclangleWidth = actualWidth / relevantSamples.Count * 0.5;
            if (relevantSamples.Count > 1)
            {
                var implicitInterval = relevantSamples[1].Begin - relevantSamples[0].Begin;
                reclangleWidth = implicitInterval.TotalMilliseconds * actualWidth / deltatimePeriod /* * 0.5*/;
            }

            foreach (var sample in relevantSamples)
            {
                {
                    var shadow = new Rectangle
                    {
                        HorizontalAlignment = HorizontalAlignment.Left,
                        VerticalAlignment = VerticalAlignment.Top,
                        Width = reclangleWidth * 0.2
                    };
                    shadow.Fill = shadow.Stroke = sample.Close > sample.Open ? Brushes.Green : Brushes.Red;
                    shadow.Opacity = 0.5;

                    var deltatime = (sample.Begin - begin).TotalMilliseconds;
                    var xPosition = deltatime * actualWidth / deltatimePeriod + reclangleWidth / 2 - reclangleWidth * 0.2 / 2;
                    //---
                    var deltaValue = sample.High - min;
                    var deltaZero = 0 - min;
                    var zeroPosition = deltaZero * actualHeight / deltaMaxMin;
                    double yPosition;
                    if (sample.Close > 0)
                    {
                        yPosition = actualHeight - (deltaValue * actualHeight / deltaMaxMin);
                    }
                    else
                    {
                        yPosition = actualHeight - zeroPosition;
                    }

                    shadow.Height = Math.Abs(sample.High - sample.Low) * actualHeight / deltaMaxMin;
                    if (shadow.Height < 0.5) shadow.Height = 0.5;
                    //---
                    shadow.Margin = new Thickness(xPosition, yPosition, 0, 0);
                    //rectangle.ToolTip =
                    //    $"{series.Name}: {sample.Begin} {sample.Open:F4} {sample.High:F4} {sample.Low:F4} {sample.Close:F4}";
                    grid.Children.Add(shadow);
                }
                ///////////////////////////////////////////////////////////////////////////////////////////
                ///
                {
                    var body = new Rectangle
                    {
                        HorizontalAlignment = HorizontalAlignment.Left,
                        VerticalAlignment = VerticalAlignment.Top,
                        Width = reclangleWidth
                    };
                    body.Fill = body.Stroke = sample.Close > sample.Open ? Brushes.Green : Brushes.Red;
                    body.Opacity = 0.5;
                    //rectangle.ToolTip = string.Format("{0}: {1} {2:F4}", series.Name, sample.Instant, sample.Value);

                    var deltatime = (sample.Begin - begin).TotalMilliseconds;
                    var xPosition = deltatime * actualWidth / deltatimePeriod;
                    //---
                    var deltaValue = sample.Close > sample.Open ? sample.Close - min : sample.Open - min;
                    var deltaZero = 0 - min;
                    var zeroPosition = deltaZero * actualHeight / deltaMaxMin;
                    double yPosition;
                    if (sample.Close > 0)
                    {
                        yPosition = actualHeight - (deltaValue * actualHeight / deltaMaxMin);
                    }
                    else
                    {
                        yPosition = actualHeight - zeroPosition;
                    }

                    body.Height = Math.Abs(sample.Close > sample.Open ? sample.Close - sample.Open : sample.Open - sample.Close) * actualHeight / deltaMaxMin;
                    if (body.Height < 0.5) body.Height = 0.5;
                    //---
                    body.Margin = new Thickness(xPosition, yPosition, 0, 0);
                    grid.Children.Add(body);
                }
            }

            return grid;
        }

        public double MaxValue {
            get { return Drawables.Max(drawable => drawable.MaxValue); }
        }

        public double MinValue {
            get { return Drawables.Min(drawable => drawable.MinValue); }
        }

        public ChartType ChartType { get; set; }
    }
}