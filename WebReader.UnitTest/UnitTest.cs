/*
   Copyright 2014 Eduardo Yuschuk (eduardo.yuschuk@gmail.com)
*/

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace WebReader.UnitTest
{
    [TestClass]
    public class UnitTest
    {
        [TestMethod]
        public void TestMethod()
        {
            string content =
                Reader.ReadString(string.Format("http://finance.yahoo.com/d/quotes.csv?s={0}&f={1}", "MSFT", "l1"));
            Assert.IsNotNull(content);
            Assert.IsTrue(float.Parse(content) > 0);
        }
    }
}