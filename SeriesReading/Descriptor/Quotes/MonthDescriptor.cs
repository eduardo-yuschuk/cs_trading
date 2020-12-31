/*
   Copyright 2014 Eduardo Yuschuk (eduardo.yuschuk@gmail.com)
*/

using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace SeriesReading.Descriptor.Quotes
{
    public class MonthDescriptor : IDescriptor
    {
        private string _monthFolder;
        private List<DayDescriptor> _dayDescriptors = new List<DayDescriptor>();

        public MonthDescriptor(string monthFolder)
        {
            _monthFolder = monthFolder;
            Name = new DirectoryInfo(_monthFolder).Name;
            Directory.EnumerateFiles(_monthFolder).ToList().ForEach(dayFile =>
            {
                _dayDescriptors.Add(new DayDescriptor(dayFile));
            });
        }

        public string Name { get; private set; }

        public string Path
        {
            get { return _monthFolder; }
        }

        public List<IDescriptor> ChildDescriptors
        {
            get { return _dayDescriptors.Cast<IDescriptor>().ToList(); }
        }

        public List<DayDescriptor> DayDescriptors
        {
            get { return _dayDescriptors; }
        }
    }
}