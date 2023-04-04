using JC.Utilities;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace UtilitiesTest
{
    [TestClass]
    public class FileHandlerTest
    {
        [TestMethod]
        public void GetTempFileNameTest()
        {
            string tempFile = FileHandler.GetTempFileName();
            Console.WriteLine(tempFile);

            string tempFile2 = FileHandler.GetTempFileName(".tt");
            Assert.IsTrue(tempFile2.EndsWith(".tt"));
        }
    }
}
