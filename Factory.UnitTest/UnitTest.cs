/*
   Copyright 2014 Eduardo Yuschuk (eduardo.yuschuk@gmail.com)
*/

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Factory.UnitTest
{
    [TestClass]
    public class UnitTest
    {
        [TestMethod]
        public void TestMethod()
        {
            var sampleInterface =
                Factory.Shared.Factory.Create<Factory.SampleLib.Shared.ISampleInterface>("ISampleInterface");

            var result = sampleInterface.SampleMethod(1, 2);

            Assert.AreEqual(3, result);
        }
    }
}