/*
   Copyright 2014 Eduardo Yuschuk (eduardo.yuschuk@gmail.com)
*/

using System;
using System.Collections.Generic;
using SeriesReading.Shared;
using FinancialSeriesUtils;
using System.IO;
using QuotesConversion;

namespace SeriesReading
{
    public class SeriesReader : ISeriesReader
    {
        private string _rootFolder;
        private DateTime _begin;
        private DateTime _end;
        private Queue<string> _paths = new Queue<string>();
        private BinaryReader _reader;

        /// <summary>
        /// Construye un Reader configurado para iterar toda la serie.
        /// </summary>
        /// <param name="rootFolder"></param>
        public SeriesReader(string rootFolder)
        {
            _rootFolder = rootFolder;
            SeriesInformation information = new SeriesInformation(_rootFolder);
            _begin = information.Begin;
            _end = information.End;
            DateTime cur = new DateTime(_begin.Year, _begin.Month, _begin.Day);
            DateTime stop = new DateTime(_end.Year, _end.Month, _end.Day).AddDays(1);
            string folderPath, filePath;
            while (cur < stop)
            {
                PathBuilder.CreatePaths(_rootFolder, cur, out folderPath, out filePath);
                _paths.Enqueue(filePath);
                cur += TimeSpan.FromDays(1);
            }
        }

        /// <summary>
        /// Construye un Reader configurado para iterar toda la serie dentro del período pasado por parámero.
        /// </summary>
        /// <param name="rootFolder"></param>
        /// <param name="begin"></param>
        /// <param name="end"></param>
        public SeriesReader(string rootFolder, DateTime begin, DateTime end)
        {
            _rootFolder = rootFolder;
            _begin = begin;
            _end = end;
            DateTime cur = new DateTime(begin.Year, begin.Month, begin.Day);
            DateTime stop = new DateTime(end.Year, end.Month, end.Day);
            string folderPath, filePath;
            while (cur <= stop)
            {
                PathBuilder.CreatePaths(_rootFolder, cur, out folderPath, out filePath);
                _paths.Enqueue(filePath);
                cur += TimeSpan.FromDays(1);
            }
        }

        private SeriesReader()
        {
        }

        /// <summary>
        /// Construye un Reader configurado para iterar un archivo (correspondiente a un día) de la serie.
        /// </summary>
        /// <param name="filePath"></param>
        public static SeriesReader CreateReaderForSingleFile(string filePath)
        {
            if (!filePath.EndsWith(".bin"))
            {
                throw new Exception("I expect a .bin file path.");
            }

            SeriesReader reader = new SeriesReader();
            reader._begin = DateTime.MinValue;
            reader._end = DateTime.MaxValue;
            reader._paths.Enqueue(filePath);
            return reader;
        }

        /// <summary>
        /// Entrega el siguiente sample considerando el período definido en la construcción de esta instancia.
        /// </summary>
        /// <param name="dateTime"></param>
        /// <param name="ask"></param>
        /// <param name="bid"></param>
        /// <returns></returns>
        public bool Next(out DateTime dateTime, out decimal ask, out decimal bid)
        {
            if (!TryToGetNext(out dateTime, out ask, out bid))
            {
                return false;
            }

            while (dateTime < _begin)
            {
                if (!TryToGetNext(out dateTime, out ask, out bid))
                {
                    return false;
                }
            }

            if (dateTime > _end) return false;
            return true;
        }

        private bool TryToGetNext(out DateTime dateTime, out decimal ask, out decimal bid)
        {
            if (!QuotesConverter.GetQuoteFromBinaryReader(_reader, out dateTime, out ask, out bid))
            {
                if (_paths.Count == 0)
                {
                    dateTime = default(DateTime);
                    ask = bid = default(decimal);
                    return false;
                }

                if (_reader != null) _reader.Close();
                string path = _paths.Dequeue();
                while (!File.Exists(path))
                {
                    if (_paths.Count > 0)
                    {
                        path = _paths.Dequeue();
                    }
                    else
                    {
                        return false;
                    }
                }

                _reader = new BinaryReader(File.OpenRead(path));
                if (!QuotesConverter.GetQuoteFromBinaryReader(_reader, out dateTime, out ask, out bid))
                {
                    throw new Exception("Empty file found.");
                }
            }

            return true;
        }
    }
}