/*
   Copyright 2014 Eduardo Yuschuk (eduardo.yuschuk@gmail.com)
*/

using System.Collections.Generic;
using System.Windows.Media;

namespace UserInterface
{
    public class DrawingColorsProvider
    {
        private readonly DrawingColors _drawingColors;
        private int _index = 0;

        private DrawingColorsProvider(DrawingColors drawingColors)
        {
            _drawingColors = drawingColors;
        }

        public static DrawingColorsProvider New => new DrawingColorsProvider(new DrawingColors());
        public Color Next => _drawingColors[_index++];

        private class DrawingColors
        {
            private readonly List<Color> _colors = new List<Color>
            {
                Colors.Brown,
                Colors.DarkGreen,
                Colors.DarkBlue,
                Colors.DarkOrange,
                Colors.CadetBlue,
                Colors.BlueViolet,
                Colors.Crimson,
                Colors.MediumAquamarine,
                Colors.DodgerBlue,
                Colors.LightSalmon,
                Colors.Olive,
                Colors.SlateGray,
            };

            public Color this[int index] => _colors[index % _colors.Count];
        }
    }
}