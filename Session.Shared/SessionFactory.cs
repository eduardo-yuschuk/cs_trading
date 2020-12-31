/*
   Copyright 2014 Eduardo Yuschuk (eduardo.yuschuk@gmail.com)
*/

namespace Session.Shared
{
    public class SessionFactory
    {
        private static Session _session;
        private static object _sessionLock = new object();

        public static Session CurrentSession
        {
            get
            {
                if (_session == null)
                {
                    lock (_sessionLock)
                    {
                        if (_session == null)
                        {
                            _session = new Session();
                        }
                    }
                }

                return _session;
            }
        }
    }
}