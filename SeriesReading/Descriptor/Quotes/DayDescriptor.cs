/*
   Copyright 2014 Eduardo Yuschuk (eduardo.yuschuk@gmail.com)
*/

using System.Collections.Generic;
using System.IO;

namespace SeriesReading.Descriptor.Quotes
{
    public class DayDescriptor : IDescriptor
    {
        private string _dayFile;

        public DayDescriptor(string dayFile)
        {
            _dayFile = dayFile;
            Name = new FileInfo(_dayFile).Name;
        }

        public string Name { get; private set; }

        public string Path
        {
            get { return _dayFile; }
        }

        public List<IDescriptor> ChildDescriptors
        {
            get { return null; }
        }
    }
}