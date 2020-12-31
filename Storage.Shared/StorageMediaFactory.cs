/*
   Copyright 2014 Eduardo Yuschuk (eduardo.yuschuk@gmail.com)
*/

namespace Storage.Shared
{
    public class StorageMediaFactory
    {
        private static IStorageMediaManager _storageMediaManager;
        private static object _storageMediaManagerLock = new object();

        public static IStorageMediaManager StorageMediaManager
        {
            get
            {
                if (_storageMediaManager == null)
                {
                    lock (_storageMediaManagerLock)
                    {
                        if (_storageMediaManager == null)
                        {
                            _storageMediaManager =
                                Factory.Shared.Factory.Create<IStorageMediaManager>("IStorageMediaManager");
                        }
                    }
                }

                return _storageMediaManager;
            }
        }

        private static IStorageMedia _defaultStorageMedia;
        private static object _defaultStorageMediaLock = new object();

        public static IStorageMedia DefaultMedia
        {
            get
            {
                if (_defaultStorageMedia == null)
                {
                    lock (_defaultStorageMediaLock)
                    {
                        if (_defaultStorageMedia == null)
                        {
                            _defaultStorageMedia = Factory.Shared.Factory.Create<IStorageMedia>("DefaultStorageMedia");
                        }
                    }
                }

                return _defaultStorageMedia;
            }
        }

        //private static IStorageContext _sessionStorageContext;
        //private static object _sessionStorageContextLock = new object();
        //public static IStorageContext DefaultStorageContext {
        //    get {
        //        if (_sessionStorageContext == null) {
        //            lock (_sessionStorageContextLock) {
        //                if (_sessionStorageContext == null) {
        //                    _sessionStorageContext = Factory.Shared.Factory.Create<IStorageContext>("SessionStorageContext");
        //                    if (!_sessionStorageContext.TryToInitialize(StorageMediaFactory.DefaultMedia)) {
        //                        throw new Exception("There was an error building the default storage context based on default media.");
        //                    }
        //                }
        //            }
        //        }
        //        return _sessionStorageContext;
        //    }
        //}
    }
}