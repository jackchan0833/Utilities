using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using Newtonsoft.Json;
using System.Drawing;
using System.Collections.Concurrent;
using System.Linq;
using NPOI.SS.UserModel;
using JC.Utilities.Excel.NPOI;

namespace JC.Utilities.Excel
{
    /// <summary>
    /// Represents the excel handler.
    /// </summary>
    public class ExcelHandler : NPOIHandler
    {
        private static ConcurrentDictionary<string, IWorkbook> _OpenedWorkBooks = new ConcurrentDictionary<string, IWorkbook>();
        private static string GetCellFormatString(ExcelCellFormat cellFormat)
        {
            if (cellFormat == ExcelCellFormat.DateTimeType || cellFormat == ExcelCellFormat.TextType)
            {
                return "@";
            }
            else if (cellFormat == ExcelCellFormat.DigitaType)
            {
                return "";
            }
            else
            {
                return "@";
            }
        }
        /// <summary>
        /// Export as excel with specified save file path.
        /// </summary>
        /// <param name="destSaveFilePath">The destination file path to save.</param>
        /// <param name="headers">The headers of work sheet.</param>
        /// <param name="rowDatas">The row data to generate.</param>
        /// <param name="listCellFormats">The cell format.</param>
        /// <param name="startDataRowIndex">The start row index to insert the row data.</param>
        /// <param name="listInsertCells">The cells to insert.</param>
        /// <param name="templateExcelFile">The excel template file, that based to generate the excel.</param>
        public static void ExportAsExcel(string destSaveFilePath, List<string> headers, List<List<string>> rowDatas,
            List<ExcelCellFormat> listCellFormats, int startDataRowIndex, List<ExcelCellInfo> listInsertCells = null, 
            string templateExcelFile = "Template.xlsx")
        {
            IWorkbook workBook = null;
            ISheet workSheet = null;
            string tempFilePath = FileHandler.GetTempFileName();
            try
            {
                string reportTemplateFilePath = FileHandler.GetFileFullPath(templateExcelFile);
                FileHandler.CopyFile(reportTemplateFilePath, tempFilePath);

                workBook = OpenWorkBook(tempFilePath);

                workSheet = workBook.GetSheetAt(0);
                int totalRowsCount = startDataRowIndex + 1 + rowDatas.Count + 10;
                int totalColumnsCount = headers.Count + 10;
                if (rowDatas.Count > 0 && rowDatas[0].Count > headers.Count)
                {
                    totalColumnsCount = rowDatas[0].Count + 10; //use max column count
                }
                var defaultCellStyle = CreateCellStyle(workBook, null, 10, "Arial", null, null, "@", null, null, null);
                CreateRowCells(workSheet, totalRowsCount, totalColumnsCount, defaultCellStyle);

                if (listInsertCells != null && listInsertCells.Any())
                {
                    foreach (var cell in listInsertCells)
                    {
                        SetCellValue(workSheet, cell.RowIndex, cell.ColumnIndex, cell.Value);
                    }
                }
                int indexColumnNo = 0;
                int indexRowNo = startDataRowIndex;
                foreach (var header in headers)
                {
                    SetCellValue(workSheet, indexRowNo, indexColumnNo, header);
                    indexColumnNo++;
                }

                int dataRowNo = indexRowNo + 1;
                //set data column format
                if (listCellFormats != null && listCellFormats.Any())
                {
                    for (int i = 0; i < listCellFormats.Count; i++)
                    {
                        var cellFormat = listCellFormats == null ? ExcelCellFormat.TextType : listCellFormats[i];
                        var numberFormat = GetCellFormatString(cellFormat);
                        var cellStyle = CreateCellStyle(workBook, System.Drawing.Color.Black, 10, "Arial", null, null, numberFormat, null, null, null);
                        SetCellStyle(workSheet, i, dataRowNo, i, dataRowNo + rowDatas.Count - 1, cellStyle);
                    }
                }

                foreach (var rowData in rowDatas)
                {
                    int dataColumnIndexNo = 0;
                    foreach (var cellVal in rowData)
                    {
                        SetCellValue(workSheet, dataRowNo, dataColumnIndexNo, cellVal);
                        dataColumnIndexNo++;
                    }
                    dataRowNo++;
                }

                SaveWorkBook(workBook, tempFilePath);
                workBook.Close();
                workBook = null;

                FileHandler.MoveFile(tempFilePath, destSaveFilePath);
            }
            finally
            {
                if (workBook != null)
                {
                    workBook.Close();
                }
                System.Threading.Thread.Sleep(1000);
                FileHandler.DeleteFile(tempFilePath);
            }
        }

        /// <summary>
        /// Close all work book.
        /// </summary>
        public static void CloseAllWorkBook()
        {
            var listKeys = _OpenedWorkBooks.Keys.ToList();
            foreach(var key in listKeys)
            {
                if (_OpenedWorkBooks.TryRemove(key, out IWorkbook workBook))
                {
                    try
                    {
                        if (workBook != null)
                        {
                            workBook.Close();
                        }
                    }
                    catch
                    {
                    }
                }
            }
        }
    }
}
