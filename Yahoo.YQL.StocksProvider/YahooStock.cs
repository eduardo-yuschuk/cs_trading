/*
   Copyright 2014 Eduardo Yuschuk (eduardo.yuschuk@gmail.com)
*/

using FinancialData.Shared;
using System;
using System.IO;
using System.Linq;
using System.Xml.Serialization;

namespace Yahoo.YQL.StocksProvider
{
    public class YahooStock
    {
        public static string RootPath = @"C:\stocks\";
        public string Name { get; set; }
        public string Symbol { get; set; }

        public string Filepath => RootPath + Symbol;

        public IBarPackage Bars { get; set; }

        public void Update()
        {
            var localInstance = Deserialize();
            Update(localInstance);
        }

        public void Update(YahooStock stock)
        {
            Name = stock.Name;
            Symbol = stock.Symbol;
            Bars = stock.Bars;
        }

        public void Update(IBarPackage barPackage)
        {
            barPackage.Samples.ForEach(bar =>
            {
                var last = Bars.Samples.Last();
                if (bar.DateTime > last.DateTime)
                {
                    Bars.Samples.Add(bar);
                }
                else if (bar.DateTime < last.DateTime)
                {
                    Bars.Samples.Insert(0, bar);
                }
                else
                {
                    throw new InvalidOperationException("Implementar...");
                }
            });
        }

        #region Serialization

        private YahooStock Deserialize()
        {
            if (File.Exists(Filepath))
            {
                XmlSerializer serializer = new XmlSerializer(typeof(YahooStock));
                using (StreamReader reader = new StreamReader(Filepath))
                {
                    YahooStock instance = (YahooStock) serializer.Deserialize(reader);
                    return instance;
                }
            }

            return new YahooStock();
        }

        public void Save()
        {
            XmlSerializer serializer = new XmlSerializer(typeof(YahooStock));
            TextWriter textWriter = new StreamWriter(Filepath);
            serializer.Serialize(textWriter, this);
            textWriter.Close();
        }

        #endregion
    }
}