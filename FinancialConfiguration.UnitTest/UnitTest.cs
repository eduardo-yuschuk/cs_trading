/*
   Copyright 2014 Eduardo Yuschuk (eduardo.yuschuk@gmail.com)
*/

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FinancialConfiguration.UnitTest
{
    [TestClass]
    public class UnitTest
    {
        [TestMethod]
        public void GetDataRoot()
        {
            Assert.IsNotNull(Configuration.Instace.DataRoot);
        }
    }
}