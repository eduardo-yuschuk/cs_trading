/*
   Copyright 2014 Eduardo Yuschuk (eduardo.yuschuk@gmail.com)
*/

namespace Storage.Shared
{
    public interface IStorageMedia
    {
        void Save(IStorageContext context, InformationUnitId informationUnit, IStorableData data);
        IStorableData Read(IStorageContext context, InformationUnitId informationUnit);
    }
}