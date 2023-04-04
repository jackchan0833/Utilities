using JC.Utilities.Excel;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;

namespace UtilitiesExcelTest
{
    [TestClass]
    public class ExcelSampleTest
    {
        [TestMethod]
        public void Export()
        {
            var listHeaders = new List<string>()
            {
                "Name",
                "Gender",
                "Age"
            };
            var listRowData = new List<List<string>>();
            listRowData.Add(new List<string>()
            {
                "John",
                "Male",
                "25"
            });
            listRowData.Add(new List<string>()
            {
                "Lucy",
                "Female",
                "20"
            });
            listRowData.Add(new List<string>()
            {
                "Tom",
                "Male",
                "18"
            });

            string generateFilePath = "Generate.xlsx";
            ExcelHandler.ExportAsExcel(generateFilePath, listHeaders, listRowData);
            bool isExists = System.IO.File.Exists(generateFilePath);
            Assert.IsTrue(isExists);
        }
    }
}
