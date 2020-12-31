/*
   Copyright 2014 Eduardo Yuschuk (eduardo.yuschuk@gmail.com)
*/

using System;
using System.IO;
using System.Xml.Serialization;

namespace TradingConfiguration.Shared
{
    [Serializable]
    public class Configuration
    {
        #region Singleton

        private static string filepath = @"C:\Configuration.xml";

        private static Configuration _instance;
        private static object _instanceLock = new object();

        public static Configuration Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (_instanceLock)
                    {
                        if (_instance == null)
                        {
                            _instance = Configuration.Deserialize();
                        }
                    }
                }

                return _instance;
            }
        }

        private static Configuration Deserialize()
        {
            if (File.Exists(filepath))
            {
                XmlSerializer serializer = new XmlSerializer(typeof(Configuration));
                using (StreamReader reader = new StreamReader(filepath))
                {
                    Configuration configuration = (Configuration) serializer.Deserialize(reader);
                    return configuration;
                }
            }

            return new Configuration();
        }

        public void Save()
        {
            XmlSerializer serializer = new XmlSerializer(typeof(Configuration));
            TextWriter textWriter = new StreamWriter(filepath);
            serializer.Serialize(textWriter, this);
            textWriter.Close();
        }

        #endregion

        private Configuration()
        {
            AssetsConfiguration = new AssetsConfiguration();
            ResearchConfiguration = new ResearchConfiguration();
            ExecutionConfiguration = new ExecutionConfiguration();
        }

        public AssetsConfiguration AssetsConfiguration { get; set; }

        public ResearchConfiguration ResearchConfiguration { get; set; }

        public ExecutionConfiguration ExecutionConfiguration { get; set; }
    }
}