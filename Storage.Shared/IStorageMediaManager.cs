/*
   Copyright 2014 Eduardo Yuschuk (eduardo.yuschuk@gmail.com)
*/

namespace Storage.Shared
{
    public interface IStorageMediaManager
    {
        void RegisterMedia(string name, IStorageMedia storageMedia);

        IStorageMedia this[string index] { get; }
    }
}