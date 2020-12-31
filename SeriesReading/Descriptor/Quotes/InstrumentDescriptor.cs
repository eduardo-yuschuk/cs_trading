/*
   Copyright 2014 Eduardo Yuschuk (eduardo.yuschuk@gmail.com)
*/

using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace SeriesReading.Descriptor.Quotes
{
    public class InstrumentDescriptor : IDescriptor
    {
        private string _instrumentFolder;
        private List<ProviderDescriptor> _providerDescriptors = new List<ProviderDescriptor>();

        public InstrumentDescriptor(string instrumentFolder)
        {
            _instrumentFolder = instrumentFolder;
            Name = new DirectoryInfo(_instrumentFolder).Name;
            Directory.EnumerateDirectories(_instrumentFolder).ToList().ForEach(providerFolder =>
            {
                _providerDescriptors.Add(new ProviderDescriptor(providerFolder));
            });
        }

        public string Name { get; private set; }

        public string Path
        {
            get { return _instrumentFolder; }
        }

        public List<IDescriptor> ChildDescriptors
        {
            get { return _providerDescriptors.Cast<IDescriptor>().ToList(); }
        }

        public List<ProviderDescriptor> ProviderDescriptors
        {
            get { return _providerDescriptors; }
        }
    }
}