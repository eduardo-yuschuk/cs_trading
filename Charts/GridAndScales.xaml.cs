/*
   Copyright 2014 Eduardo Yuschuk (eduardo.yuschuk@gmail.com)
*/

using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace Charts
{
    /// <summary>
    /// Interaction logic for GridAndScales.xaml
    /// </summary>
    public partial class GridAndScales : UserControl
    {
        public GridAndScales()
        {
            InitializeComponent();
        }

        private DateTime? _begin = null;
        private DateTime? _end = null;

        public DateTime Begin {
            set {
                _begin = value;
                ComposeAxes();
            }
        }

        public DateTime End {
            set {
                _end = value;
                ComposeAxes();
            }
        }

        private double? _maximum = null;
        private double? _minimum = null;

        public double Maximum {
            set {
                _maximum = value;
                ComposeAxes();
            }
        }

        public double Minimum {
            set {
                _minimum = value;
                ComposeAxes();
            }
        }

        public int YSteps = 4;

        protected override void OnRenderSizeChanged(SizeChangedInfo sizeInfo)
        {
            ComposeAxes();
        }

        private void ComposeAxes()
        {
            RootPanel.Children.Clear();

            #region Y

            if (_minimum.HasValue && _maximum.HasValue)
            {
                var deltaY = _maximum.Value - _minimum.Value;
                var stepAmount = deltaY / YSteps;
                for (var step = 1; step < YSteps; step++)
                {
                    var stepDisplayValue = _minimum.Value + stepAmount * step;
                    var stepValue = stepAmount * step;
                    var y = stepValue * ActualHeight / deltaY;
                    y = ActualHeight - y;

                    var line = new Line()
                    {
                        X1 = 0,
                        X2 = ActualWidth,
                        Y1 = y,
                        Y2 = y,
                        StrokeThickness = 1,
                        Stroke = Brushes.Gray,
                        StrokeDashArray = new DoubleCollection(new[] { 4.0, 4.0, }),
                        Opacity = 0.7,
                    };
                    RootPanel.Children.Add(line);

                    var text = new TextBlock()
                    {
                        Text = stepDisplayValue.ToString("0.00000"),
                        FontFamily = new FontFamily("Calibri"),
                        FontSize = 10,
                        Background = Brushes.White,
                        //Opacity = 0.5,
                        Padding = new Thickness(2, 0, 0, 0),
                        Margin = new Thickness(0, y - 7, 0, 0),
                        HorizontalAlignment = HorizontalAlignment.Left,
                        VerticalAlignment = VerticalAlignment.Top,
                    };
                    RootPanel.Children.Add(text);
                }
            }

            #endregion

            #region X

            if (_begin.HasValue && _end.HasValue)
            {
                long ticksPeriod = (_end.Value - _begin.Value).Ticks;
                int year = _begin.Value.Year + 1;
                while (year <= _end.Value.Year)
                {
                    DateTime dt = new DateTime(year, 1, 1);
                    long ticksYear = (dt - _begin.Value).Ticks;
                    double x = ticksYear * ActualWidth / ticksPeriod;

                    Line line = new Line()
                    {
                        X1 = x,
                        X2 = x,
                        Y1 = 0,
                        Y2 = ActualHeight,
                        StrokeThickness = 1,
                        Stroke = Brushes.Gray,
                        StrokeDashArray = new DoubleCollection(new[] { 4.0, 4.0, }),
                        Opacity = 0.7,
                    };
                    RootPanel.Children.Add(line);

                    TextBlock text = new TextBlock()
                    {
                        Text = year.ToString(),
                        FontFamily = new FontFamily("Calibri"),
                        FontSize = 10,
                        Background = Brushes.White,
                        Opacity = 0.5,
                        Margin = new Thickness(x - 10, ActualHeight - 12, 0, 0),
                        HorizontalAlignment = HorizontalAlignment.Left,
                        VerticalAlignment = VerticalAlignment.Top,
                    };
                    RootPanel.Children.Add(text);

                    year++;
                }
            }

            #endregion
        }
    }
}