/*
   Copyright 2014 Eduardo Yuschuk (eduardo.yuschuk@gmail.com)
*/

namespace Storage.Shared
{
    public interface IStorageContext
    {
        string Name { get; }
        void Save(InformationUnitId informationUnit, IStorableData data);
        IStorableData Read(InformationUnitId informationUnit);
        bool TryToInitialize(string name, IStorageMedia storageMedia);
    }
}