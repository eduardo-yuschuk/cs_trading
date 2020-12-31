/*
   Copyright 2014 Eduardo Yuschuk (eduardo.yuschuk@gmail.com)
*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace Charts
{
    public partial class Chart
    {
        public Chart()
        {
            InitializeComponent();
        }

        public const double DeltaBySide = 0.1;
        public bool FixedScales { get; set; }

        private readonly List<Series> _series = new List<Series>();

        //public string ChartType { get; set; }
        public string ChartTitle {
            get => ChartTitleLabel.Text;
            set => ChartTitleLabel.Text = value;
        }

        public double FindAbsoluteMax()
        {
            var max = (double)int.MinValue;//double.MinValue;
            _series.ForEach(series =>
            {
                if (series.Drawables.Count > 0)
                {
                    var amax = series.Drawables.FindAll(x => x.Begin >= Begin && x.End <= End)
                        .Max(x => x.MaxValue);
                    if (amax > max) max = amax;
                }
            });
            return max;
        }

        public double FindAbsoluteMin()
        {
            var min = (double)int.MaxValue;//double.MaxValue;
            _series.ForEach(series =>
            {
                if (series.Drawables.Count > 0)
                {
                    var amin = series.Drawables.FindAll(x => x.Begin >= Begin && x.End <= End)
                        .Where(x => x.MinValue > 0).Min(x => x.MinValue);
                    if (amin < min) min = amin;
                }
            });
            return min;
        }

        public void AddSeries(Series series)
        {
            if (series.IsRenderable)
            {
                //this.trend.ChartType = this.ChartType;
                _series.Add(series);
                Dispatcher.Invoke(() =>
                {
                    if (!FixedScales)
                    {
                        var max = FindAbsoluteMax();
                        var min = FindAbsoluteMin();
                        var delta = max - min;
                        Maximum = max + DeltaBySide * delta;
                        Minimum = min - DeltaBySide * delta;
                        trend.RemoveAllSeries();
                    }

                    _series.ForEach(x => trend.AddSeries(x));
                });
            }
            else
            {
                throw new Exception("no es drawable");
            }
        }

        public void RemoveSeries(Series series)
        {
            var fseries = _series.FirstOrDefault(aseries => aseries.Name == series.Name);
            if (fseries != null)
            {
                _series.Remove(fseries);
                Dispatcher.Invoke(() => trend.RemoveAllSeries());
                if (_series.Count > 0)
                {
                    Dispatcher.Invoke(() =>
                    {
                        if (!FixedScales)
                        {
                            var max = FindAbsoluteMax();
                            var min = FindAbsoluteMin();
                            var delta = max - min;
                            Maximum = max + DeltaBySide * delta;
                            Minimum = min - DeltaBySide * delta;
                            //this.trend.RemoveAllSeries();
                        }

                        _series.ForEach(x => trend.AddSeries(x));
                    });
                }
            }
        }

        public void ClearSeries()
        {
            _series.ToList().ForEach(fseries =>
            {
                if (fseries != null)
                {
                    _series.Remove(fseries);
                    Dispatcher.Invoke(() => trend.RemoveAllSeries());
                    if (_series.Count > 0)
                    {
                        Dispatcher.Invoke(() =>
                        {
                            if (!FixedScales)
                            {
                                var max = FindAbsoluteMax();
                                var min = FindAbsoluteMin();
                                var delta = max - min;
                                Maximum = max + DeltaBySide * delta;
                                Minimum = min - DeltaBySide * delta;
                                //this.trend.RemoveAllSeries();
                            }

                            _series.ForEach(x => trend.AddSeries(x));
                        });
                    }
                }
            });
        }

        public DateTime Begin {
            get => trend.Begin;
            set => gridAndScales.Begin = trend.Begin = value;
        }

        public DateTime End {
            get => trend.End;
            set => gridAndScales.End = trend.End = value;
        }

        public double Maximum {
            get => trend.Maximum;
            set => gridAndScales.Maximum = trend.Maximum = value;
        }

        public double Minimum {
            get => trend.Minimum;
            set => gridAndScales.Minimum = trend.Minimum = value;
        }

        #region Mouse events

        private void Grid_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed
                && (!ZoomCheckBox.IsChecked.HasValue || !ZoomCheckBox.IsChecked.Value))
            {
                var actualPosition = e.GetPosition(this);
                MoveTrend(_origin, actualPosition);
                _origin = actualPosition;
            }
            else if (e.RightButton == MouseButtonState.Pressed
                     || (e.LeftButton == MouseButtonState.Pressed
                         && ZoomCheckBox.IsChecked.HasValue && ZoomCheckBox.IsChecked.Value))
            {
                var actualPosition = e.GetPosition(this);
                ZoomTrend(_origin, actualPosition);
                _origin = actualPosition;
            }
        }

        private Point _origin;

        private void Grid_MouseDown(object sender, MouseButtonEventArgs e)
        {
            _origin = e.GetPosition(this);
        }

        private void Grid_MouseUp(object sender, MouseButtonEventArgs e)
        {
        }

        private void MoveTrend(Point from, Point to)
        {
            Begin = MoveX(Begin, from.X, to.X);
            End = MoveX(End, from.X, to.X);
            Maximum = MoveY(Maximum, from.Y, to.Y);
            Minimum = MoveY(Minimum, from.Y, to.Y);
            trend.Redraw();
        }

        private void ZoomTrend(Point from, Point to)
        {
            ZoomX(from.X, to.X);
            ZoomY(from.Y, to.Y);
            trend.Redraw();
        }

        private DateTime MoveX(DateTime dateTime, double from, double to)
        {
            var movement = to - from;
            var screenMilliseconds = (End - Begin).TotalMilliseconds;
            var movementMilliseconds = movement * screenMilliseconds / ActualWidth;
            return dateTime.AddMilliseconds(-movementMilliseconds);
        }

        private double MoveY(double value, double from, double to)
        {
            var movement = to - from;
            var screenAmount = Maximum - Minimum;
            var movementAmount = movement * screenAmount / ActualHeight;
            return value + movementAmount;
        }

        private void ZoomX(double from, double to)
        {
            var movement = to - from;
            var screenMilliseconds = (End - Begin).TotalMilliseconds;
            var movementMilliseconds = (movement * screenMilliseconds / ActualWidth) / 2;
            Begin = Begin.AddMilliseconds(movementMilliseconds);
            End = End.AddMilliseconds(-movementMilliseconds);
        }

        private void ZoomY(double from, double to)
        {
            var movement = to - from;
            var screenAmount = Maximum - Minimum;
            var movementAmount = (movement * screenAmount / ActualHeight) / 2;
            Maximum += movementAmount;
            Minimum -= movementAmount;
        }

        #endregion
    }
}