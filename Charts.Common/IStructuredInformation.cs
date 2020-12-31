/*
   Copyright 2014 Eduardo Yuschuk (eduardo.yuschuk@gmail.com)
*/

using System.Collections.Generic;

namespace Charts.Common
{
    public interface IStructuredInformation
    {
        string Header { get; }
        List<IInformationRow> InformationRows { get; }
    }
}