/*
   Copyright 2014 Eduardo Yuschuk (eduardo.yuschuk@gmail.com)
*/

using Microsoft.VisualStudio.TestTools.UnitTesting;
using Session.Shared;
using Storage.Shared;

namespace Session.UnitTest
{
    [TestClass]
    public class UnitTest
    {
        [TestMethod]
        public void TestMethod()
        {
            IStorageContext sessionStorageContext =
                SessionFactory.CurrentSession.GetStorageContext(InformationUnitType.Temporal);
            // creo una unidad de información
            InformationUnitId informationUnit = new InformationUnitId
            {
                Name = "SampleData",
                Type = InformationUnitType.Temporal,
            };
            // creo los datos para esa unidad de información
            SampleStorableData sampleData = new SampleStorableData();
            for (int i = 0; i < 100; i++)
            {
                sampleData.AddValue(i);
            }

            // los almaceno en el storage context
            sessionStorageContext.Save(informationUnit, sampleData);
            // los recupero del storage context
            SampleStorableData recoveredSampleData =
                new SampleStorableData(sessionStorageContext.Read(informationUnit));
            for (int i = 0; i < 100; i++)
            {
                Assert.AreEqual(sampleData[i], recoveredSampleData[i]);
            }
        }
    }
}