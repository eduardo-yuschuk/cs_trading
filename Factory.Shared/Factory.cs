/*
   Copyright 2014 Eduardo Yuschuk (eduardo.yuschuk@gmail.com)
*/

using Configuration.Shared;
using Factory.Lib;
using System;
using System.Collections.Generic;

namespace Factory.Shared
{
    public static class Factory
    {
        #region Fields

        private static readonly Dictionary<string, object> _instances = new Dictionary<string, object>();

        #endregion

        #region Constructors

        static Factory()
        {
            _instances = new Dictionary<string, object>();
        }

        #endregion

        public static T Create<T>(bool isSingleton = false)
        {
            return Create<T>(typeof(T).Name, isSingleton);
        }

        public static T Create<T>(string configurationEntry, bool isSingleton = false)
        {
            // TODO remove ConfigurationManager dependency (must depend on Configuration)
            string typeName = ConfigurationFactory.Configuration[configurationEntry];

            if (typeName == null)
            {
                throw new Exception(string.Format("Entry {0} not found in app.config", configurationEntry));
            }

            return TypeBuilder.CreateType<T>(typeName, isSingleton);
        }
    }
}