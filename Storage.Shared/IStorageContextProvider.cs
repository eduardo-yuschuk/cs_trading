/*
   Copyright 2014 Eduardo Yuschuk (eduardo.yuschuk@gmail.com)
*/

namespace Storage.Shared
{
    public interface IStorageContextProvider
    {
        IStorageContext GetStorageContext(InformationUnitType informationUnitType);
    }
}