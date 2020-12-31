/*
   Copyright 2014 Eduardo Yuschuk (eduardo.yuschuk@gmail.com)
*/

using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace SeriesReading.Descriptor.Quotes
{
    public class ProviderDescriptor : IDescriptor
    {
        private string _providerFolder;
        private List<YearDescriptor> _yearsDescriptor = new List<YearDescriptor>();

        public ProviderDescriptor(string providerFolder)
        {
            _providerFolder = providerFolder;
            Name = new DirectoryInfo(_providerFolder).Name;
            Directory.EnumerateDirectories(_providerFolder).ToList().ForEach(yearFolder =>
            {
                _yearsDescriptor.Add(new YearDescriptor(yearFolder));
            });
        }

        public string Name { get; private set; }

        public string Path
        {
            get { return _providerFolder; }
        }

        public List<IDescriptor> ChildDescriptors
        {
            get { return _yearsDescriptor.Cast<IDescriptor>().ToList(); }
        }

        public List<YearDescriptor> YearDescriptors
        {
            get { return _yearsDescriptor; }
        }
    }
}