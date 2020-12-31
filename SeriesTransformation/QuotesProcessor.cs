/*
   Copyright 2014 Eduardo Yuschuk (eduardo.yuschuk@gmail.com)
*/

using System;
using SeriesTransformation.Shared;
using System.IO;
using FinancialSeriesUtils;
using QuotesConversion;
using static QuotesConversion.QuotesConverter;

namespace SeriesTransformation
{
    public class QuotesProcessor : IQuotesProcessor
    {
        private char[] _separator = new char[] {','};
        private string _rootFolder;
        private DateTime _epoch = new DateTime(1970, 1, 1);
        private int _dayOfYear = -1;
        private BinaryWriter _writer;
        public IQuoteDateTimeParser QuoteDateTimeParser { get; private set; }

        public QuotesProcessor(string targetFolderPath, IQuoteDateTimeParser parser)
        {
            _rootFolder = targetFolderPath;
            QuoteDateTimeParser = parser;
        }

        public void StoreQuoteFromString(string text)
        {
            long time;
            DateTime dateTime;
            decimal ask, bid;
            QuotesConverter.GetQuoteFromString(text, out dateTime, out time, out ask, out bid, QuoteDateTimeParser);
            if (_dayOfYear != dateTime.DayOfYear)
            {
                if (_writer != null)
                {
                    _writer.Close();
                }

                string folderPath;
                string filePath;
                PathBuilder.CreatePaths(_rootFolder, dateTime, out folderPath, out filePath);
                Directory.CreateDirectory(folderPath);
                _writer = new BinaryWriter(File.Create(filePath));
                _dayOfYear = dateTime.DayOfYear;
            }

            _writer.Write((Int64) time);
            _writer.Write(ask);
            _writer.Write(bid);
        }
    }
}