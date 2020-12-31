/*
   Copyright 2014 Eduardo Yuschuk (eduardo.yuschuk@gmail.com)
*/

using System.Net;

namespace WebReader
{
    public class Reader
    {
        public static string ReadString(string url)
        {
            using (var client = new WebClient())
            {
                var stream = client.OpenRead(url);
                var reader = new System.IO.StreamReader(stream);
                return reader.ReadToEnd();
            }
        }
    }
}