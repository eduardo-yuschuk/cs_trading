/*
   Copyright 2014 Eduardo Yuschuk (eduardo.yuschuk@gmail.com)
*/

using System;
using System.Xml.Serialization;
using System.IO;

namespace SeriesAcquisition.Shared
{
    [Serializable]
    public class VerificationReport
    {
        public static VerificationReport LoadFromFile(string filename)
        {
            return Deserialize(filename);
        }

        public void SaveToFile(string filename)
        {
            Serialize(filename);
        }

        #region Serialization

        private void Serialize(string filename)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(VerificationReport));
            TextWriter textWriter = new StreamWriter(filename);
            serializer.Serialize(textWriter, this);
            textWriter.Close();
        }

        private static VerificationReport Deserialize(string filename)
        {
            if (File.Exists(filename))
            {
                XmlSerializer deserializer = new XmlSerializer(typeof(VerificationReport));
                TextReader textReader = new StreamReader(filename);
                VerificationReport config = (VerificationReport) deserializer.Deserialize(textReader);
                textReader.Close();
                return config;
            }

            return null;
        }

        #endregion

        public bool Verified { get; set; }
        public bool TransformationCompleted { get; set; }
    }
}