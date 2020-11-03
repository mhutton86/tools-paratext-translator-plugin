﻿using System;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TvpMain.Check;

namespace TvpTest
{
    [TestClass]

    public class CheckAndFixTests
    {
        [TestMethod]
        public void TestFileSerialization()
        {
            var testCafItem = new CheckAndFixItem()
            {
                Name = "testName",
                Description = "testDescription",
                Version = "1.0.0",
                CheckRegex = @"\b\w*z+\w*\b",
                CheckScript = @"function(input){
return input;
}",
                FixRegex = @"\b\w*z+\w*\b",
                FixScript = @"function(input){
return input;
}",
            };

            var testSavePath = Path.Combine(@"C:\Users\mark\AppData\Local\Temp", "tempCafItem1.xml");

            testCafItem.SaveToXmlFile(testSavePath);

            var loadedCafItem = CheckAndFixItem.LoadFromXmlFile(testSavePath);
            Assert.AreEqual(testCafItem, loadedCafItem);
        }
    }
}
