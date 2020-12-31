/*
   Copyright 2014 Eduardo Yuschuk (eduardo.yuschuk@gmail.com)
*/

using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace SeriesReading.Descriptor.Quotes
{
    public class YearDescriptor : IDescriptor
    {
        private string _yearFolder;
        private List<MonthDescriptor> _monthDescriptors = new List<MonthDescriptor>();

        public YearDescriptor(string yearFolder)
        {
            _yearFolder = yearFolder;
            Name = new DirectoryInfo(_yearFolder).Name;
            Directory.EnumerateDirectories(_yearFolder).ToList().ForEach(monthFolder =>
            {
                _monthDescriptors.Add(new MonthDescriptor(monthFolder));
            });
        }

        public string Name { get; private set; }

        public string Path
        {
            get { return _yearFolder; }
        }

        public List<IDescriptor> ChildDescriptors
        {
            get { return _monthDescriptors.Cast<IDescriptor>().ToList(); }
        }

        public List<MonthDescriptor> MonthDescriptors
        {
            get { return _monthDescriptors; }
        }
    }
}