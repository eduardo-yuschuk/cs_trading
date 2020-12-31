/*
   Copyright 2014 Eduardo Yuschuk (eduardo.yuschuk@gmail.com)
*/

using System;
using System.Collections.Generic;
using FinancialSeriesUtils;

namespace BarsBuilder.Automation
{
    public class Robot
    {
        public void Start()
        {
            new List<TimeSpan>
            {
                FinancialTimeSpans.M1,
                //FinancialTimeSpans.M5,
                //FinancialTimeSpans.M15
            }.ForEach(BarsFileBuilder.Create);
        }
    }
}