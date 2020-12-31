/*
   Copyright 2014 Eduardo Yuschuk (eduardo.yuschuk@gmail.com)
*/

namespace Configuration.Shared
{
    public interface IConfiguration
    {
        string this[string index] { get; }
    }
}