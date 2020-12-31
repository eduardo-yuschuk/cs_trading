/*
   Copyright 2014 Eduardo Yuschuk (eduardo.yuschuk@gmail.com)
*/

using System;
using System.Collections.Generic;
using System.Linq;

namespace FinancialSeriesUtils
{
    public class FinancialTimeSpans
    {
        public static TimeSpan S1
        {
            get { return TimeSpan.FromSeconds(10); }
        }

        public static TimeSpan S30
        {
            get { return TimeSpan.FromSeconds(30); }
        }

        public static TimeSpan M1
        {
            get { return TimeSpan.FromMinutes(1); }
        }

        public static TimeSpan M2
        {
            get { return TimeSpan.FromMinutes(2); }
        }

        public static TimeSpan M3
        {
            get { return TimeSpan.FromMinutes(3); }
        }

        public static TimeSpan M4
        {
            get { return TimeSpan.FromMinutes(4); }
        }

        public static TimeSpan M5
        {
            get { return TimeSpan.FromMinutes(5); }
        }

        public static TimeSpan M10
        {
            get { return TimeSpan.FromMinutes(10); }
        }

        public static TimeSpan M15
        {
            get { return TimeSpan.FromMinutes(15); }
        }

        public static TimeSpan M20
        {
            get { return TimeSpan.FromMinutes(20); }
        }

        public static TimeSpan M25
        {
            get { return TimeSpan.FromMinutes(25); }
        }

        public static TimeSpan M30
        {
            get { return TimeSpan.FromMinutes(30); }
        }

        public static TimeSpan M40
        {
            get { return TimeSpan.FromMinutes(40); }
        }

        public static TimeSpan M50
        {
            get { return TimeSpan.FromMinutes(50); }
        }

        public static TimeSpan H1
        {
            get { return TimeSpan.FromHours(1); }
        }

        public static TimeSpan H2
        {
            get { return TimeSpan.FromHours(2); }
        }

        public static TimeSpan H3
        {
            get { return TimeSpan.FromHours(3); }
        }

        public static TimeSpan H5
        {
            get { return TimeSpan.FromHours(5); }
        }

        private static List<TimeSpan> _seconds = new List<TimeSpan>
        {
            S1,
            S30,
        };

        private static List<TimeSpan> _minutes = new List<TimeSpan>
        {
            M1,
            M2,
            M3,
            M4,
            M5,
            M10,
            M15,
            M20,
            M25,
            M30,
            M40,
            M50,
        };

        private static List<TimeSpan> _hours = new List<TimeSpan>
        {
            H1,
            H2,
            H3,
            H5,
        };

        public static List<TimeSpan> Seconds
        {
            get { return _seconds; }
        }

        public static List<TimeSpan> Minutes
        {
            get { return _minutes; }
        }

        public static List<TimeSpan> Hours
        {
            get { return _hours; }
        }

        private static List<TimeSpan> _all;

        public static List<TimeSpan> All
        {
            get
            {
                if (_all == null)
                    _all = Seconds.Concat(Minutes).Concat(Hours).ToList();
                return _all;
            }
        }
    }
}