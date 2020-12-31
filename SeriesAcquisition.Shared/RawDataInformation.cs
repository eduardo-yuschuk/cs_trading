/*
   Copyright 2014 Eduardo Yuschuk (eduardo.yuschuk@gmail.com)
*/

using System;
using System.IO;

namespace SeriesAcquisition.Shared
{
    public class RawDataInformation
    {
        public DateTime Begin { get; set; }
        public DateTime End { get; set; }

        private RawDataInformation()
        {
        }

        public static RawDataInformation FromPath(string filename)
        {
            if (File.Exists(filename))
            {
                string[] parts =
                    new FileInfo(filename).Name.Split(new string[] {"__", "."}, StringSplitOptions.RemoveEmptyEntries);
                return new RawDataInformation
                {
                    Begin = DateTime.ParseExact(parts[0], "yyyy_MM_dd_HH_mm_ss", null),
                    End = DateTime.ParseExact(parts[1], "yyyy_MM_dd_HH_mm_ss", null),
                };
            }

            return null;
        }
    }
}