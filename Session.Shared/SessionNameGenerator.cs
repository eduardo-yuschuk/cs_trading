/*
   Copyright 2014 Eduardo Yuschuk (eduardo.yuschuk@gmail.com)
*/

using System;

namespace Session.Shared
{
    public class SessionNameGenerator
    {
        public static string CreateName()
        {
            return DateTime.UtcNow.ToString("yyyy.MM.dd.hh.mm.ss.ffffff");
        }
    }
}