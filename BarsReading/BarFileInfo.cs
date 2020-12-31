/*
   Copyright 2014 Eduardo Yuschuk (eduardo.yuschuk@gmail.com)
*/

using System;
using System.IO;
using FinancialData;

// ReSharper disable CommentTypo

namespace BarsReading
{
    public class BarFileInfo
    {
        public string FilePath { get; private set; }
        public TimeSpan TimeFrame { get; private set; }
        public long FirstQuoteBarIndex { get; private set; }
        public long SamplesInFile { get; private set; }
        public long LastQuoteBarIndex { get; private set; }
        public DateTime FirstBarDateTime { get; private set; }
        public DateTime LastBarDateTime { get; private set; }

        public BarFileInfo(string filePath, TimeSpan timeFrame)
        {
            FilePath = filePath;
            TimeFrame = timeFrame;
            var fileInfo = new FileInfo(filePath);
            var fileNameParts = fileInfo.Name.Split(new[] {'_', '.'}, StringSplitOptions.RemoveEmptyEntries);
            FirstQuoteBarIndex = long.Parse(fileNameParts[4]);
            var samplesInFileRemainder = fileInfo.Length % (double) Bar.BinarySize;
            if (samplesInFileRemainder > 0)
                throw new Exception("Corrupted file.");
            SamplesInFile = fileInfo.Length / Bar.BinarySize;
            // -1 porque entre los samples se encuentra el primero
            LastQuoteBarIndex = FirstQuoteBarIndex + SamplesInFile - 1;
            FirstBarDateTime = new DateTime(TimeFrame.Ticks * FirstQuoteBarIndex);
            var bar = BarsReader.ReadBarFromFile(TimeFrame, filePath);
            if (FirstBarDateTime != bar.DateTime)
            {
                throw new Exception("Corrupted file.");
            }

            LastBarDateTime = new DateTime(TimeFrame.Ticks * LastQuoteBarIndex);
        }
    }
}