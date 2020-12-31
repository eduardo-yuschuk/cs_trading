/*
   Copyright 2014 Eduardo Yuschuk (eduardo.yuschuk@gmail.com)
*/

using System;
using System.Collections.Generic;
using Storage.Shared;

namespace Session.Shared
{
    public class Session : IStorageContextProvider
    {
        private object _contextsLock = new object();

        private Dictionary<InformationUnitType, IStorageContext> _storageContexts =
            new Dictionary<InformationUnitType, IStorageContext>();

        public string Name { get; private set; }

        public Session()
        {
            Name = SessionNameGenerator.CreateName();
        }

        public IStorageContext GetStorageContext(InformationUnitType informationUnitType)
        {
            if (!_storageContexts.ContainsKey(informationUnitType))
            {
                lock (_contextsLock)
                {
                    if (!_storageContexts.ContainsKey(informationUnitType))
                    {
                        _storageContexts[informationUnitType] =
                            Factory.Shared.Factory.Create<IStorageContext>("SessionStorageContext");
                        if (!_storageContexts[informationUnitType].TryToInitialize(informationUnitType.ToString(),
                            StorageMediaFactory.DefaultMedia))
                        {
                            throw new Exception(
                                "There was an error building the default storage context based on default media.");
                        }
                    }
                }
            }

            return _storageContexts[informationUnitType];
        }
    }
}