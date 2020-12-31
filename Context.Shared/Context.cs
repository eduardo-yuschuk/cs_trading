/*
   Copyright 2014 Eduardo Yuschuk (eduardo.yuschuk@gmail.com)
*/

using System;
using Storage.Shared;

namespace Context.Shared
{
    public class Context : IStorageContextProvider
    {
        private object _contextsLock = new object();

        //private Dictionary<InformationUnitType, IStorageContext> _storageContexts = new Dictionary<InformationUnitType, IStorageContext>();

        public UniqueContextType ContextType { get; private set; }

        //public Subcontexts

        public Context(UniqueContextType contextType)
        {
            ContextType = contextType;
        }

        public IStorageContext GetStorageContext(InformationUnitType informationUnitType)
        {
            //if (!_storageContexts.ContainsKey(informationUnitType)) {
            //    lock (_contextsLock) {
            //        if (!_storageContexts.ContainsKey(informationUnitType)) {
            //            string key = ContextType.ToString() + "StorageContext";
            //            _storageContexts[informationUnitType] = Factory.Shared.Factory.Create<IStorageContext>(key);
            //            if (!_storageContexts[informationUnitType].TryToInitialize(informationUnitType.ToString(), StorageMediaFactory.DefaultMedia)) {
            //                throw new Exception("There was an error building the default storage context based on default media.");
            //            }
            //        }
            //    }
            //}
            //return _storageContexts[informationUnitType];

            throw new NotImplementedException();
        }
    }
}