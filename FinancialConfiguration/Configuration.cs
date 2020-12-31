/*
   Copyright 2014 Eduardo Yuschuk (eduardo.yuschuk@gmail.com)
*/

using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;

namespace FinancialConfiguration
{
    [Serializable]
    public class Configuration
    {
        private const string _configurationFile = @"..\..\..\ConfigurationFiles\FinancialConfiguration.xml";

        private static Configuration _instance;

        private static object _instanceLock = new object();

        public static Configuration Instace
        {
            get
            {
                if (_instance == null)
                    lock (_instanceLock)
                        if (_instance == null)
                        {
                            if (File.Exists(_configurationFile))
                            {
                                _instance = Deserialize(_configurationFile);
                            }
                            else
                            {
                                _instance = new Configuration();
                                _instance.Initialize();
                            }

                            // serializo para actualizar el xml con los campos inicializados por default en el ctor
                            _instance.Serialize(_configurationFile);
                        }

                return _instance;
            }
        }

        private void Serialize(string _configurationFile)
        {
            try
            {
                XmlSerializer serializer = new XmlSerializer(typeof(Configuration));
                TextWriter textWriter = new StreamWriter(_configurationFile);
                serializer.Serialize(textWriter, this);
                textWriter.Close();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                Serialize(_configurationFile);
            }
        }

        private static Configuration Deserialize(string _configurationFile)
        {
            XmlSerializer deserializer = new XmlSerializer(typeof(Configuration));
            TextReader textReader = new StreamReader(_configurationFile);
            Configuration config = (Configuration) deserializer.Deserialize(textReader);
            textReader.Close();
            return config;
        }

        public string DataRoot { get; set; }

        public Configuration()
        {
        }

        private void Initialize()
        {
            DataRoot = @"C:\quotes\";
            Providers = new List<string> {"Dukascopy"};
            Instruments = new List<string> {"EUR/USD"};
            StartYear = 2000;
            JavaPath = @"C:\Program Files\Java\jre8\bin\java.exe";
            JarsFolder = @"C:\hfsystem\Code\ExternalDependencies\java";
        }

        public List<string> Providers { get; set; }

        public List<string> Instruments { get; set; }

        public int StartYear { get; set; }

        public string JavaPath { get; set; }

        public string JarsFolder { get; set; }
    }
}