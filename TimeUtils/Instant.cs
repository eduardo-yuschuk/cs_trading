/*
   Copyright 2014 Eduardo Yuschuk (eduardo.yuschuk@gmail.com)
*/

using System;

namespace TimeUtils
{
    public class Instant
    {
        private static DateTime _epoch = new DateTime(1970, 1, 1);

        public static DateTime Now
        {
            get { return DateTime.UtcNow; }
        }

        public static DateTime FromMillisAfterEpoch(long millis)
        {
            return _epoch.AddMilliseconds(millis);
        }
    }
}