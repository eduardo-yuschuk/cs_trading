/*
   Copyright 2014 Eduardo Yuschuk (eduardo.yuschuk@gmail.com)
*/

using Configuration.Shared;
using System.Configuration;

namespace Configuration.Lib.AppConfig
{
    public class Configuration : IConfiguration
    {
        public string this[string index]
        {
            get { return ConfigurationManager.AppSettings[index]; }
        }
    }
}