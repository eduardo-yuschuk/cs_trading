/*
   Copyright 2014 Eduardo Yuschuk (eduardo.yuschuk@gmail.com)
*/

using System;
using System.IO;

namespace IndicatorsReading
{
    /// <summary>
    /// Hay que recordar que la información de indicadores se basa en una serie original de bars. De ahí que 
    /// guarde relación en el nombrado de variables con aspectos de las bars.
    /// </summary>
    public class IndicatorFileInfo
    {
        public string Name { get; private set; }
        public int Period { get; private set; }
        public string FilePath { get; private set; }
        public TimeSpan TimeFrame { get; private set; }
        private FileInfo _fileInfo;
        public long FirstQuoteBarIndex { get; private set; }
        public long SamplesInFile { get; private set; }
        public long LastQuoteBarIndex { get; private set; }
        public DateTime FirstBarDateTime { get; private set; }
        public DateTime LastBarDateTime { get; private set; }

        public IndicatorFileInfo(string filePath, TimeSpan timeFrame)
        {
            FilePath = filePath;
            TimeFrame = timeFrame;
            _fileInfo = new FileInfo(filePath);
            var fileNameParts = _fileInfo.Name.Split(new char[] {'_', '.'}, StringSplitOptions.RemoveEmptyEntries);
            Name = fileNameParts[0];
            Period = int.Parse(fileNameParts[1]);
            if (timeFrame.Ticks != long.Parse(fileNameParts[3])) throw new Exception("Invalid timeFrame.");
            FirstQuoteBarIndex = long.Parse(fileNameParts[5]);
            var samplesInFileRemainder = _fileInfo.Length % (double) sizeof(decimal);
            if (samplesInFileRemainder > 0)
                throw new Exception("Corrupted file.");
            SamplesInFile = _fileInfo.Length / sizeof(decimal);
            // -1 porque entre los samples se encuentra el primero
            LastQuoteBarIndex = FirstQuoteBarIndex + (long) SamplesInFile - 1;

            FirstBarDateTime = new DateTime(TimeFrame.Ticks * FirstQuoteBarIndex);
            LastBarDateTime = new DateTime(TimeFrame.Ticks * LastQuoteBarIndex);
        }
    }
}