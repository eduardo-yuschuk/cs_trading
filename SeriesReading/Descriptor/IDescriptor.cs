/*
   Copyright 2014 Eduardo Yuschuk (eduardo.yuschuk@gmail.com)
*/

using System.Collections.Generic;

namespace SeriesReading.Descriptor
{
    public interface IDescriptor
    {
        string Name { get; }
        string Path { get; }
        List<IDescriptor> ChildDescriptors { get; }
    }
}