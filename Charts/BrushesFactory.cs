/*
   Copyright 2014 Eduardo Yuschuk (eduardo.yuschuk@gmail.com)
*/

using System.Collections.Generic;
using System.Windows.Media;

namespace Charts
{
    public class BrushesFactory
    {
        static int index = 0;

        static List<Brush> BrushesList = new List<Brush>()
        {
            new SolidColorBrush(Color.FromRgb(208, 118, 30)),
            new SolidColorBrush(Color.FromRgb(14, 20, 179)),
            //new SolidColorBrush(Color.FromRgb(216,195,88)),
            new SolidColorBrush(Color.FromRgb(109, 8, 57)),
            //new SolidColorBrush(Color.FromRgb(208,231,153)),
            new SolidColorBrush(Color.FromRgb(37, 39, 30)),
            new SolidColorBrush(Color.FromRgb(145, 36, 178)),
            new SolidColorBrush(Color.FromRgb(95, 189, 171)),
            new SolidColorBrush(Color.FromRgb(115, 54, 18)),
            new SolidColorBrush(Color.FromRgb(184, 107, 223)),

            new SolidColorBrush(Color.FromRgb(197, 42, 12)),
            new SolidColorBrush(Color.FromRgb(9, 41, 107)),

            new SolidColorBrush(Color.FromRgb(13, 135, 228)),
            new SolidColorBrush(Color.FromRgb(8, 102, 127)),
            //new SolidColorBrush(Color.FromRgb()),
            //new SolidColorBrush(Color.FromRgb()),
            new SolidColorBrush(Color.FromRgb(208, 118, 30)),
            new SolidColorBrush(Color.FromRgb(14, 20, 179)),
            //new SolidColorBrush(Color.FromRgb(216,195,88)),
            new SolidColorBrush(Color.FromRgb(109, 8, 57)),
            //new SolidColorBrush(Color.FromRgb(208,231,153)),
            new SolidColorBrush(Color.FromRgb(37, 39, 30)),
            new SolidColorBrush(Color.FromRgb(145, 36, 178)),
            new SolidColorBrush(Color.FromRgb(95, 189, 171)),
            new SolidColorBrush(Color.FromRgb(115, 54, 18)),
            new SolidColorBrush(Color.FromRgb(184, 107, 223)),

            new SolidColorBrush(Color.FromRgb(197, 42, 12)),
            new SolidColorBrush(Color.FromRgb(9, 41, 107)),

            new SolidColorBrush(Color.FromRgb(13, 135, 228)),
            new SolidColorBrush(Color.FromRgb(8, 102, 127)),
            new SolidColorBrush(Color.FromRgb(208, 118, 30)),
            new SolidColorBrush(Color.FromRgb(14, 20, 179)),
            //new SolidColorBrush(Color.FromRgb(216,195,88)),
            new SolidColorBrush(Color.FromRgb(109, 8, 57)),
            //new SolidColorBrush(Color.FromRgb(208,231,153)),
            new SolidColorBrush(Color.FromRgb(37, 39, 30)),
            new SolidColorBrush(Color.FromRgb(145, 36, 178)),
            new SolidColorBrush(Color.FromRgb(95, 189, 171)),
            new SolidColorBrush(Color.FromRgb(115, 54, 18)),
            new SolidColorBrush(Color.FromRgb(184, 107, 223)),

            new SolidColorBrush(Color.FromRgb(197, 42, 12)),
            new SolidColorBrush(Color.FromRgb(9, 41, 107)),

            new SolidColorBrush(Color.FromRgb(13, 135, 228)),
            new SolidColorBrush(Color.FromRgb(8, 102, 127)),
        };

        public static Brush Next
        {
            get
            {
                if (index == 36) index = 0;
                return BrushesList[index++];
            }
        }
    }
}