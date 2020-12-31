/*
   Copyright 2014 Eduardo Yuschuk (eduardo.yuschuk@gmail.com)
*/

using Storage.Shared;
using System.Collections.Generic;

namespace Storage
{
    public class StorageMediaManager : IStorageMediaManager
    {
        Dictionary<string, IStorageMedia> _media = new Dictionary<string, IStorageMedia>();

        public void RegisterMedia(string name, IStorageMedia storageMedia)
        {
            _media[name] = storageMedia;
        }

        public IStorageMedia this[string index]
        {
            get { return _media[index]; }
        }
    }
}