using NPOI.HSSF.UserModel;
using NPOI.HSSF.Util;
using NPOI.SS.UserModel;
using NPOI.SS.Util;
using NPOI.XSSF.UserModel;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;

namespace JC.Utilities.Excel.NPOI
{
    /// <summary>
    /// Represents the <see cref="NPOIHandler"/>.
    /// </summary>
    public class NPOIHandler
    {
        /// <summary>
        /// Creates a new workbook.
        /// </summary>
        /// <returns></returns>
        public static IWorkbook CreateNewWorkbook()
        {
            IWorkbook workBook = new XSSFWorkbook(XSSFWorkbookType.XLSX);
            return workBook;
        }
        /// <summary>
        /// Open file as workbook.
        /// </summary>
        /// <param name="filePath">The specified file to open.</param>
        /// <param name="fileExtension">The excel file extension. Working fine with '.xlsx' file extension, but some issue on 'xls' file.</param>
        /// <returns></returns>
        public static IWorkbook OpenWorkBook(string filePath, string fileExtension = null)
        {
            IWorkbook workBook = null;
            using (FileStream file = new FileStream(filePath, FileMode.Open, FileAccess.ReadWrite))
            {
                try
                {
                    workBook = new XSSFWorkbook(file);
                }
                catch
                {
                    if (!string.IsNullOrWhiteSpace(fileExtension))
                    {
                        if (fileExtension.ToLower() == ".xls")
                        {
                            workBook = new HSSFWorkbook(file);
                        }
                        else
                        {
                            throw;
                        }
                    }
                    else
                    {
                        workBook = new HSSFWorkbook(file);
                    }
                }
            }
            return workBook;
        }
        /// <summary>
        /// Save specified workbook to destionation file.
        /// </summary>
        /// <param name="workBook">The specified workbook to save.</param>
        /// <param name="destFilePath">The destination file.</param>
        public static void SaveWorkBook(IWorkbook workBook, string destFilePath)
        {
            using (FileStream file = new FileStream(destFilePath, FileMode.Create, FileAccess.ReadWrite))
            {
                workBook.Write(file);
            }
        }
        /// <summary>
        /// Deletes all worksheets from specified workbook.
        /// </summary>
        /// <param name="workBook"></param>
        public static void DeleteAllSheets(IWorkbook workBook)
        {
            int totalSheetCount = GetWorksheetCount(workBook);
            for(int i = totalSheetCount - 1; i >= 0; i--)
            {
                workBook.RemoveSheetAt(i);
            }
        }
        /// <summary>
        /// Gets worksheet count of specified workbook.
        /// </summary>
        /// <param name="workBook">The workbook to counter.</param>
        /// <returns>The count of worksheets.</returns>
        public static int GetWorksheetCount(IWorkbook workBook)
        {
            int count = 0;
            var sheets = workBook.GetEnumerator();
            while (sheets.MoveNext())
            {
                count++;
            }
            return count;
        }
        /// <summary>
        /// Creates cells for specified worksheet.
        /// </summary>
        /// <param name="workBook">The relative workbook of worksheet.</param>
        /// <param name="workSheet">The specified worksheet.</param>
        /// <param name="totalRowsCount">The total count of rows to create.</param>
        /// <param name="totalColumnsCount">The total count of columns to create.</param>
        /// <param name="fontSize">The font size of cell.</param>
        /// <param name="fontName">The font name of cell.</param>
        /// <param name="fontColor">The font color of cell.</param>
        /// <param name="dataFormat">The data format of cell.</param>
        public static void CreateRowCells(IWorkbook workBook, ISheet workSheet, int totalRowsCount, int totalColumnsCount, int? fontSize = null, string fontName = null, System.Drawing.Color? fontColor = null, string dataFormat = null)
        {
            IFont font = workBook.CreateFont();
            if (fontSize != null)
            {
                font.FontHeightInPoints = fontSize.Value;
            }
            if (!string.IsNullOrWhiteSpace(fontName))
            {
                font.FontName = fontName;
            }
            if (fontColor != null)
            {
                SetFontColor(workBook, font, fontColor);
            }
            ICellStyle cellStyle = workBook.CreateCellStyle();
            cellStyle.SetFont(font);

            if (!string.IsNullOrWhiteSpace(dataFormat))
            {
                IDataFormat format = workBook.CreateDataFormat();
                cellStyle.DataFormat = format.GetFormat(dataFormat);
            }
            CreateRowCells(workSheet, totalRowsCount, totalColumnsCount, cellStyle);
        }
        /// <summary>
        /// Creates cells for specified worksheet.
        /// </summary>
        /// <param name="workSheet">The specified worksheet.</param>
        /// <param name="totalRowsCount">The total count of rows to create.</param>
        /// <param name="totalColumnsCount">The total count of columns to create.</param>
        /// <param name="cellStyle">The cell style to create.</param>
        /// <param name="offsetRowCount">The offset row count before to create cell.</param>
        /// <param name="offsetColumnCount">The offset column count before to create cell.</param>
        /// <param name="overwriteExistCell">Whether overwrite existing cell when create.</param>
        public static void CreateRowCells(ISheet workSheet, int totalRowsCount, int totalColumnsCount, ICellStyle cellStyle, int offsetRowCount = 0, int offsetColumnCount = 0, bool overwriteExistCell = true)
        {
            if (overwriteExistCell)
            {
                for (int i = offsetRowCount; i < (totalRowsCount + offsetRowCount); i++)
                {
                    var row = workSheet.CreateRow(i);
                    for (int j = offsetColumnCount; j < totalColumnsCount; j++)
                    {
                        var cell = row.CreateCell(j);
                        cell.CellStyle = cellStyle;
                    }
                }
            }
            else
            {

                for (int i = offsetRowCount; i < (totalRowsCount + offsetRowCount); i++)
                {
                    var row = workSheet.GetRow(i);
                    if (row == null)
                    {
                        row = workSheet.CreateRow(i);
                    }
                    for (int j = offsetColumnCount; j < totalColumnsCount; j++)
                    {
                        var cell = row.GetCell(j);
                        if (cell == null)
                        {
                            cell = row.CreateCell(j);
                        }
                        cell.CellStyle = cellStyle;
                    }
                }
            }
        }
        /// <summary>
        /// Sets the cell's style.
        /// </summary>
        /// <param name="workSheet">The specified worksheet of cell.</param>
        /// <param name="startColumnIndex">The start column index to set.</param>
        /// <param name="startRowIndex">The start row index to set.</param>
        /// <param name="endColumnIndex">The end column index to set.</param>
        /// <param name="endRowIndex">The end row index to set.</param>
        /// <param name="cellStyle">The cell style.</param>
        public static void SetCellStyle(ISheet workSheet, int startColumnIndex, int startRowIndex, int endColumnIndex, int endRowIndex, ICellStyle cellStyle)
        {
            for (int j = startRowIndex; j <= endRowIndex; j++)
            {
                var row = workSheet.GetRow(j);
                if (row == null)
                {
                    continue;
                }
                for (int i = startColumnIndex; i <= endColumnIndex; i++)
                {
                    var cell = row.GetCell(i);
                    if (cell == null)
                    {
                        continue;
                    }
                    cell.CellStyle = cellStyle;
                }
            }

        }
        /// <summary>
        /// Gets the recognize letter 'A' of specified column.
        /// </summary>
        /// <param name="columnIndex">The specified column.</param>
        /// <returns>The column letter.</returns>
        public static string GetColumnLetter(int columnIndex)
        {
            int columnNumber = columnIndex;
            int baseValue = (int)'Z' - (int)'A' + 1;
            string columnName = String.Empty;
            do
            {
                columnName = Convert.ToChar('A' + columnNumber % baseValue) + columnName;
                columnNumber = columnNumber / baseValue - 1;
            }
            while (columnNumber >= 0);

            return columnName;
        }
        /// <summary>
        /// Sets the cell border's style.
        /// </summary>
        public static void SetCellBorder(IWorkbook workBook, ISheet workSheet, string startColumnName, int startRowIndex, string endColumnName, int endRowIndex)
        {
            int startColumnIndex = GetColumnIndex(startColumnName);
            int endColumnIndex = GetColumnIndex(endColumnName);
            SetCellBorder(workBook, workSheet, startColumnIndex, startRowIndex, endColumnIndex, endRowIndex);
        }
        /// <summary>
        /// Sets the cell border's style.
        /// </summary>
        public static void SetCellBorder(IWorkbook workBook, ISheet workSheet, int startColumnIndex, int startRowIndex, int endColumnIndex, int endRowIndex)
        {
            for (int j = startRowIndex; j <= endRowIndex; j++)
            {
                var row = workSheet.GetRow(j);
                if (row == null)
                {
                    continue;
                }
                for (int i = startColumnIndex; i <= endColumnIndex; i++)
                {
                    var cell = row.GetCell(i);
                    if (cell == null)
                    {
                        continue;
                    }
                    var cellStyle = workBook.CreateCellStyle();
                    cellStyle.CloneStyleFrom(cell.CellStyle);
                    cellStyle.BorderBottom = BorderStyle.Thin;
                    cellStyle.BorderLeft = BorderStyle.Thin;
                    cellStyle.BorderRight = BorderStyle.Thin;
                    cellStyle.BorderTop = BorderStyle.Thin;
                    cellStyle.BottomBorderColor = IndexedColors.Black.Index;
                    cellStyle.LeftBorderColor = IndexedColors.Black.Index;
                    cellStyle.RightBorderColor = IndexedColors.Black.Index;
                    cellStyle.TopBorderColor = IndexedColors.Black.Index;
                    cell.CellStyle = cellStyle;
                }
            }
        }
        /// <summary>
        /// Sets the cell's background.
        /// </summary>
        public static void SetCellBackground(IWorkbook workBook, ISheet workSheet, string startColumnName, int startRowIndex, string endColumnName, int endRowIndex, System.Drawing.Color color)
        {
            int startColumnIndex = GetColumnIndex(startColumnName);
            int endColumnIndex = GetColumnIndex(endColumnName);
            SetCellBackground(workBook, workSheet, startColumnIndex, startRowIndex, endColumnIndex, endRowIndex, color);
        }
        public static void SetCellBackground(IWorkbook workBook, ISheet workSheet, int startColumnIndex, int startRowIndex, int endColumnIndex, int endRowIndex, System.Drawing.Color color)
        {
            GetExcelColor(workBook, color, out bool isXSSF, out XSSFColor excelColor, out short colorIndex);

            for (int j = startRowIndex; j <= endRowIndex; j++)
            {
                var row = workSheet.GetRow(j);
                if (row == null)
                {
                    continue;
                }
                for (int i = startColumnIndex; i <= endColumnIndex; i++)
                {
                    var cell = row.GetCell(i);
                    if (cell == null)
                    {
                        continue;
                    }
                    if (isXSSF)
                    {
                        var xCellStyle = (XSSFCellStyle)workBook.CreateCellStyle();
                        xCellStyle.CloneStyleFrom(cell.CellStyle);
                        xCellStyle.FillPattern = FillPattern.SolidForeground;
                        xCellStyle.FillBackgroundXSSFColor = excelColor;
                        xCellStyle.SetFillForegroundColor(excelColor);
                        cell.CellStyle = xCellStyle;

                    }
                    else
                    {
                        var xCellStyle = (HSSFCellStyle)workBook.CreateCellStyle();
                        xCellStyle.CloneStyleFrom(cell.CellStyle);
                        xCellStyle.FillPattern = FillPattern.SolidForeground;
                        xCellStyle.FillBackgroundColor = colorIndex;
                        xCellStyle.FillForegroundColor = colorIndex;
                        cell.CellStyle = xCellStyle;
                    }
                }
            }
        }
        /// <summary>
        /// Sets the cell's background.
        /// </summary>
        public static ICellStyle SetCellBackground(IWorkbook workBook, ICellStyle cellStyle, System.Drawing.Color color)
        {
            GetExcelColor(workBook, color, out bool isXSSF, out XSSFColor excelColor, out short colorIndex);
            return SetCellBackground(cellStyle, isXSSF, excelColor, colorIndex);
        }
        /// <summary>
        /// Sets the cell's background.
        /// </summary>
        public static ICellStyle SetCellBackground(ICellStyle cellStyle, bool isXSSF, XSSFColor excelXSSFColor, short hSSFColorIndex)
        {
            if (isXSSF)
            {
                var xCellStyle = (XSSFCellStyle)cellStyle;
                xCellStyle.FillPattern = FillPattern.SolidForeground;
                xCellStyle.FillBackgroundXSSFColor = excelXSSFColor;
                xCellStyle.SetFillForegroundColor(excelXSSFColor);
                return xCellStyle;

            }
            else
            {
                var xCellStyle = (HSSFCellStyle)cellStyle;
                xCellStyle.FillPattern = FillPattern.SolidForeground;
                xCellStyle.FillBackgroundColor = hSSFColorIndex;
                xCellStyle.FillForegroundColor = hSSFColorIndex;
                return xCellStyle;
            }
        }

        /// <summary>
        /// Sets specified cell's value.
        /// </summary>
        public static void SetCellValue(ISheet workSheet, string startColumnName, int startRowIndex, string endColumnName, int endRowIndex, object value)
        {
            int startColumnIndex = GetColumnIndex(startColumnName);
            int endColumnIndex = GetColumnIndex(endColumnName);
            for (int i = startColumnIndex; i <= endColumnIndex; i++)
            {
                for (int j = startRowIndex; j <= endRowIndex; j++)
                {
                    SetCellValue(workSheet, j, i, value);
                }
            }
        }
        /// <summary>
        /// Sets specified cell's value.
        /// </summary>
        public static void SetCellValue(ISheet workSheet, int rowIndex, int columnIndex, object value)
        {
            var row = workSheet.GetRow(rowIndex);
            var cell = row.GetCell(columnIndex);
            if (cell == null)
            {
                return;
            }
            if (value is string)
            {
                cell.SetCellValue((string)value);
            }
            else if (value is decimal)
            {
                cell.SetCellValue(Convert.ToDouble(value));
            }
            else if (value is double)
            {
                cell.SetCellValue((double)value);
            }
            else if (value is int)
            {
                cell.SetCellValue(Convert.ToDouble(value));
            }
            else
            {
                cell.SetCellValue((string)value);
            }
        }
        /// <summary>
        /// Sets specified cell's value.
        /// </summary>
        public static void SetCellValue(ISheet workSheet, string columnName, int rowIndex, object value)
        {
            int columnIndex = GetColumnIndex(columnName);
            SetCellValue(workSheet, rowIndex, columnIndex, value);
        }
        /// <summary>
        /// Sets row's height.
        /// </summary>
        public static void SetRowHeight(ISheet workSheet, int rowIndex, short height)
        {
            var row = workSheet.GetRow(rowIndex);
            row.Height = height;
        }
        /// <summary>
        /// Sets cell's width.
        /// </summary>
        public static void SetColumnWidth(ISheet workSheet, string columnName, int width)
        {
            int columnIndex = GetColumnIndex(columnName);
            SetColumnWidth(workSheet, columnIndex, width);
        }
        /// <summary>
        /// Sets cell's width.
        /// </summary>
        public static void SetColumnWidth(ISheet workSheet, int columnIndex, int width)
        {
            workSheet.SetColumnWidth(columnIndex, width);
        }
        /// <summary>
        /// Sets cell to auto fit the size.
        /// </summary>
        public static void SetColumnAutoFit(ISheet workSheet, string columnName)
        {
            int columnIndex = GetColumnIndex(columnName);
            SetColumnAutoFit(workSheet, columnIndex);
        }
        /// <summary>
        /// Sets cell to auto fit the size.
        /// </summary>
        public static void SetColumnAutoFit(ISheet workSheet, int columnIndex)
        {
            workSheet.AutoSizeColumn(columnIndex);
        }
        /// <summary>
        /// Sets cell with specified number format.
        /// </summary>
        public static void SetCellNumberFormat(IWorkbook workBook, ISheet workSheet, string startColumnName, int startRowIndex, string endColumnName, int endRowIndex, string numberFormat)
        {
            int startColumnIndex = GetColumnIndex(startColumnName);
            int endColumnIndex = GetColumnIndex(endColumnName);
            for (int i = startColumnIndex; i <= endColumnIndex; i++)
            {
                for (int j = startRowIndex; j <= endRowIndex; j++)
                {
                    SetCellNumberFormat(workBook, workSheet, j, i, numberFormat);
                }
            }
        }
        /// <summary>
        /// Sets cell with specified number format.
        /// </summary>
        public static void SetCellNumberFormat(IWorkbook workBook, ISheet workSheet, string columnName, int rowIndex, string numberFormat)
        {
            int columnIndex = GetColumnIndex(columnName);
            SetCellNumberFormat(workBook, workSheet, rowIndex, columnIndex, numberFormat);
        }
        /// <summary>
        /// Sets cell with specified number format.
        /// </summary>
        public static void SetCellNumberFormat(IWorkbook workBook, ISheet workSheet, int rowIndex, int columnIndex, string numberFormat)
        {
            var row = workSheet.GetRow(rowIndex);
            var cell = row.GetCell(columnIndex);
            if (cell == null)
            {
                return;
            }
            IDataFormat format = workBook.CreateDataFormat();
            CellUtil.SetCellStyleProperty(cell, CellUtil.DATA_FORMAT, format.GetFormat(numberFormat));
        }

        /// <summary>
        /// Sets cell formula to specified cell.
        /// </summary>
        public static void SetCellFormula(ISheet workSheet, string columnName, int rowIndex, string cellFormula)
        {
            int columnIndex = GetColumnIndex(columnName);
            SetCellFormula(workSheet, rowIndex, columnIndex, cellFormula);
        }
        /// <summary>
        /// Sets cell formula to specified cell.
        /// </summary>
        public static void SetCellFormula(ISheet workSheet, int rowIndex, int columnIndex, string cellFormula)
        {
            var row = workSheet.GetRow(rowIndex);
            var cell = row.GetCell(columnIndex);
            if (cell == null)
            {
                return;
            }
            cell.SetCellType(CellType.Formula);
            cell.SetCellFormula(cellFormula?.TrimStart('='));
        }

        /// <summary>
        /// Creates a new font style and it to workbook's font style table, and set it to specified cell.
        /// </summary>
        public static void SetCellFont(IWorkbook workBook, ISheet workSheet, string startColumnName, int startRowIndex, string endColumnName, int endRowIndex, System.Drawing.Color? fontColor, int? fontSize, string fontName, bool? isBlod)
        {
            int startColumnIndex = GetColumnIndex(startColumnName);
            int endColumnIndex = GetColumnIndex(endColumnName);
            SetCellFont(workBook, workSheet, startColumnIndex, startRowIndex, endColumnIndex, endRowIndex, fontColor, fontSize, fontName, isBlod);
        }
        /// <summary>
        /// Creates a new font style and it to workbook's font style table, and set it to specified cell.
        /// </summary>
        public static void SetCellFont(IWorkbook workBook, ISheet workSheet, int startColumnIndex, int startRowIndex, int endColumnIndex, int endRowIndex, System.Drawing.Color? fontColor, int? fontSize, string fontName, bool? isBlod)
        {
            for (int i = startColumnIndex; i <= endColumnIndex; i++)
            {
                for (int j = startRowIndex; j <= endRowIndex; j++)
                {
                    SetCellFont(workBook, workSheet, j, i, fontColor, fontSize, fontName, isBlod);
                }
            }
        }
        /// <summary>
        /// Creates a new font style and it to workbook's font style table, and set it to specified cell.
        /// </summary>
        public static void SetCellFont(IWorkbook workBook, ISheet workSheet, string columnName, int rowIndex, System.Drawing.Color? fontColor, int? fontSize, string fontName, bool? isBlod)
        {
            int columnIndex = GetColumnIndex(columnName);
            SetCellFont(workBook, workSheet, rowIndex, columnIndex, fontColor, fontSize, fontName, isBlod);
        }
        /// <summary>
        /// Creates a new font style and it to workbook's font style table, and set it to specified cell.
        /// </summary>
        public static void SetCellFont(IWorkbook workBook, ISheet workSheet, int rowIndex, int columnIndex, System.Drawing.Color? fontColor, int? fontSize, string fontName, bool? isBlod)
        {
            var row = workSheet.GetRow(rowIndex);
            var cell = row.GetCell(columnIndex);
            if (cell == null)
            {
                return;
            }
            var cellStyle = workBook.CreateCellStyle();
            cellStyle.CloneStyleFrom(cell.CellStyle);

            var font = workBook.CreateFont();
            font.CloneStyleFrom(cellStyle.GetFont(workBook));
            if (fontColor != null)
            {
                SetFontColor(workBook, font, fontColor);
            }
            if (fontSize != null)
            {
                font.FontHeightInPoints = fontSize.Value;
            }
            if (!string.IsNullOrWhiteSpace(fontName))
            {
                font.FontName = fontName;
            }
            if (isBlod != null)
            {
                font.IsBold = isBlod.Value;
            }
            cellStyle.SetFont(font);
            cell.CellStyle = cellStyle;
        }
        /// <summary>
        /// Creates a new cell style and it to workbook's style table. 
        /// </summary>
        public static ICellStyle CreateCellStyle(IWorkbook workBook, System.Drawing.Color? fontColor, int? fontSize, string fontName, bool? isBlod, bool? hasBorder, string dataFormat, System.Drawing.Color? backgroundColor,
            bool? isHorizontalCenter, bool? isVerticalCenter)
        {
            var cellStyle = workBook.CreateCellStyle();

            var font = workBook.CreateFont();
            font.CloneStyleFrom(cellStyle.GetFont(workBook));
            if (fontColor != null)
            {
                SetFontColor(workBook, font, fontColor);
            }
            if (fontSize != null)
            {
                font.FontHeightInPoints = fontSize.Value;
            }
            if (!string.IsNullOrWhiteSpace(fontName))
            {
                font.FontName = fontName;
            }
            if (!string.IsNullOrWhiteSpace(dataFormat))
            {
                IDataFormat format = workBook.CreateDataFormat();
                cellStyle.DataFormat = format.GetFormat(dataFormat);
            }
            if (isBlod != null)
            {
                font.IsBold = isBlod.Value;
            }
            if (hasBorder == true)
            {
                cellStyle.BorderBottom = BorderStyle.Thin;
                cellStyle.BorderLeft = BorderStyle.Thin;
                cellStyle.BorderRight = BorderStyle.Thin;
                cellStyle.BorderTop = BorderStyle.Thin;
                cellStyle.BottomBorderColor = IndexedColors.Black.Index;
                cellStyle.LeftBorderColor = IndexedColors.Black.Index;
                cellStyle.RightBorderColor = IndexedColors.Black.Index;
                cellStyle.TopBorderColor = IndexedColors.Black.Index;
            }
            if (backgroundColor != null)
            {
                cellStyle = SetCellBackground(workBook, cellStyle, backgroundColor.Value);
            }
            cellStyle.SetFont(font);

            if (isHorizontalCenter == true)
            {
                cellStyle.Alignment = HorizontalAlignment.Center;
            }
            if (isVerticalCenter == true)
            {
                cellStyle.VerticalAlignment = VerticalAlignment.Center;
            }
            return cellStyle;
        }
        /// <summary>
        /// Sets specified font's color.
        /// </summary>
        public static void SetFontColor(IWorkbook workBook, IFont font, System.Drawing.Color? fontColor)
        {
            if (fontColor == null)
            {
                return;
            }
            var color = fontColor.Value;
            bool isXSSFWorkbook = workBook is XSSFWorkbook;
            if (isXSSFWorkbook)
            {
                var xFont = (XSSFFont)font;
                xFont.SetColor(new XSSFColor(color));
                font = xFont;
            }
            else
            {
                HSSFPalette palette = ((HSSFWorkbook)workBook).GetCustomPalette();
                palette.SetColorAtIndex(HSSFColor.Teal.Index, color.R, color.G, color.B);
                font.Color = HSSFColor.Teal.Index;
            }
        }
        /// <summary>
        /// Hidden specified column.
        /// </summary>
        public static void HiddenColumn(ISheet workSheet, string columnName, bool hidden = true)
        {
            int columnIndex = GetColumnIndex(columnName);
            HiddenColumn(workSheet, columnIndex, hidden);
        }
        /// <summary>
        /// Hidden specified column.
        /// </summary>
        public static void HiddenColumn(ISheet workSheet, int columnIndex, bool hidden = true)
        {
            workSheet.SetColumnHidden(columnIndex, hidden);
        }
        /// <summary>
        /// Hidden specified columns.
        /// </summary>
        public static void HiddenColumns(ISheet workSheet, int startColumnIndex, int endColumnIndex, bool isHidden)
        {
            for (int i = startColumnIndex; i <= endColumnIndex; i++)
            {
                HiddenColumn(workSheet, i, isHidden);
            }
        }
        /// <summary>
        /// Gets specified cell.
        /// </summary>
        public static ICell GetCell(ISheet workSheet, int rowIndex, int columnIndex)
        {
            var row = workSheet.GetRow(rowIndex);
            if (row == null)
            {
                return null;
            }
            return row.GetCell(columnIndex);
        }
        /// <summary>
        /// Gets specified cell's style.
        /// </summary>
        public static ICellStyle GetCellStyle(ISheet workSheet, int rowIndex, int columnIndex)
        {
            var row = workSheet.GetRow(rowIndex);
            if (row == null)
            {
                return null;
            }
            var cell = row.GetCell(columnIndex);
            if (cell == null)
            {
                return null;
            }
            return cell.CellStyle;
        }
        /// <summary>
        /// Gets specified cell value's text.
        /// </summary>
        public static string GetCellValueText(CellValue cellValue)
        {
            if (cellValue == null)
            {
                return null;
            }
            if (cellValue.CellType == CellType.String)
            {
                return cellValue.StringValue?.Trim();
            }
            else if (cellValue.CellType == CellType.Numeric)
            {
                return cellValue.NumberValue.ToString();
            }
            else if (cellValue.CellType == CellType.Boolean)
            {
                return cellValue.BooleanValue.ToString();
            }
            else if (cellValue.CellType == CellType.Formula)
            {
                return cellValue.NumberValue.ToString();
            }
            else
            {
                return cellValue.StringValue?.Trim();
            }
        }
        /// <summary>
        /// Gets specified cell's text.
        /// </summary>
        public static string GetCellText(ICell cell)
        {
            if (cell == null)
            {
                return null;
            }
            if (cell.CellType == CellType.String)
            {
                return cell.StringCellValue?.Trim();
            }
            else if (cell.CellType == CellType.Numeric)
            {
                return cell.NumericCellValue.ToString();
            }
            else if (cell.CellType == CellType.Boolean)
            {
                return cell.BooleanCellValue.ToString();
            }
            else if (cell.CellType == CellType.Formula)
            {
                return cell.NumericCellValue.ToString();
            }
            else
            {
                return cell.StringCellValue?.Trim();
            }
        }
        /// <summary>
        /// Gets specified cell's text.
        /// </summary>
        public static string GetCellText(ISheet workSheet, int rowIndex, int columnIndex)
        {
            var row = workSheet.GetRow(rowIndex);
            if (row == null)
            {
                return null;
            }
            var cell = row.GetCell(columnIndex);
            if (cell == null)
            {
                return null;
            }
            return GetCellText(cell);
        }
        /// <summary>
        /// Gets specified cell's text.
        /// </summary>
        public static string GetCellText(ISheet workSheet, string columnName, int rowIndex)
        {
            int columnIndex = GetColumnIndex(columnName);
            return GetCellText(workSheet, rowIndex, columnIndex);
        }
        /// <summary>
        /// Sets specified cell's text alignment.
        /// </summary>
        public static void SetCellTextAlignment(ISheet workSheet, string startColumnName, int startRowIndex, string endColumnName, int endRowIndex, bool isHorizontalCenter = true, bool isVerticalCenter = false)
        {
            int startColumnIndex = GetColumnIndex(startColumnName);
            int endColumnIndex = GetColumnIndex(endColumnName);
            for (int i = startColumnIndex; i <= endColumnIndex; i++)
            {
                for (int j = startRowIndex; j <= endRowIndex; j++)
                {
                    SetCellTextAlignment(workSheet, j, i, isHorizontalCenter, isVerticalCenter);
                }
            }
        }
        /// <summary>
        /// Sets specified cell's text alignment.
        /// </summary>
        public static void SetCellTextAlignment(ISheet workSheet, int rowIndex, int columnIndex, bool isHorizontalCenter = true, bool isVerticalCenter = false)
        {
            var row = workSheet.GetRow(rowIndex);
            var cell = row.GetCell(columnIndex);
            if (cell == null)
            {
                return;
            }
            if (isHorizontalCenter)
            {
                CellUtil.SetAlignment(cell, HorizontalAlignment.Center);
            }
            if (isVerticalCenter)
            {
                CellUtil.SetVerticalAlignment(cell, VerticalAlignment.Center);
            }
        }
        /// <summary>
        /// Sets specified cell's text alignment.
        /// </summary>
        public static void SetCellTextAlignment(ISheet workSheet, string columnName, int rowIndex, bool isHorizontalCenter = true, bool isVerticalCenter = false)
        {
            int columnIndex = GetColumnIndex(columnName);
            SetCellTextAlignment(workSheet, rowIndex, columnIndex, isHorizontalCenter, isVerticalCenter);
        }
        /// <summary>
        /// Merge the specified cells.
        /// </summary>
        public static void MergeCell(ISheet workSheet, int startColumnIndex, int startRowIndex, int endColumnIndex, int endRowIndex)
        {
            if (startColumnIndex == endColumnIndex && startRowIndex == endRowIndex)
            {
                return;
            }
            var range = new CellRangeAddress(startRowIndex, endRowIndex, startColumnIndex, endColumnIndex);
            workSheet.AddMergedRegion(range);
        }
        /// <summary>
        /// Merge the specified cells.
        /// </summary>
        public static void MergeCell(ISheet workSheet, string startColumnName, int startRowIndex, string endColumnName, int endRowIndex)
        {
            int startColumnIndex = GetColumnIndex(startColumnName);
            int endColumnIndex = GetColumnIndex(endColumnName);
            MergeCell(workSheet, startColumnIndex, startRowIndex, endColumnIndex, endRowIndex);
        }
        /// <summary>
        /// Freeze the specified rows.
        /// </summary>
        public static void FrezeeRows(ISheet workSheet, int topRows)
        {
            // Freeze just one row
            //sheet1.CreateFreezePane(0, 1, 0, 1);
            workSheet.CreateFreezePane(0, 1, 0, topRows);
        }
        /// <summary>
        /// Freeze the specified columns.
        /// </summary>
        public static void FrezeeColumns(ISheet workSheet, int leftColumnCount)
        {
            // Freeze just one column
            //sheet2.CreateFreezePane(1, 0, 1, 0);
            workSheet.CreateFreezePane(1, 0, leftColumnCount, 0);
        }
        /// <summary>
        /// Freeze the specified columns and rows.
        /// </summary>
        /// <param name="workSheet">The specified worksheet.</param>
        /// <param name="rowCount">The freeze rows.</param>
        /// <param name="columnCount">The freeze columns.</param>
        public static void FrezeeRowColumns(ISheet workSheet, int rowCount, int columnCount)
        {
            workSheet.CreateFreezePane(columnCount, rowCount);
        }
        /// <summary>
        /// Evalucate all cells value of specified workbook.
        /// </summary>
        /// <param name="workBook"></param>
        public static void EvaluateAllFormula(IWorkbook workBook)
        {
            workBook.GetCreationHelper().CreateFormulaEvaluator().EvaluateAll(); //计算公式
        }
        /// <summary>
        /// Evalucate the cell value of specified cell.
        /// </summary>
        /// <param name="workBook">The relative workbook.</param>
        /// <param name="cell">The cell to evalucate.</param>
        /// <returns>The string value of cell.</returns>
        public static string EvaluateCellFormula(IWorkbook workBook, ICell cell)
        {
            var eval = workBook.GetCreationHelper().CreateFormulaEvaluator();
            var cellValue = eval.Evaluate(cell);
            return GetCellValueText(cellValue);
        }
        /// <summary>
        /// Gets column index with specified column name 'A'.
        /// </summary>
        /// <param name="columnName">The specified column name.</param>
        /// <returns>The column index.</returns>
        public static int GetColumnIndex(string columnName)
        {
            int columnIndex = 0;
            string upperCellName = columnName.ToUpper();
            int baseValue = (int)'Z' - (int)'A' + 1;
            int mod = 1;
            for (int i = upperCellName.Length - 1; i >= 0; i--)
            {
                columnIndex += (upperCellName[i] - 'A' + 1) * mod;
                mod *= baseValue;
            }
            return columnIndex - 1;
        }
        private static void GetExcelColor(IWorkbook workBook, System.Drawing.Color color, out bool isXSSF, out XSSFColor excelColor, out short colorIndex)
        {
            isXSSF = workBook is XSSFWorkbook;
            excelColor = null;
            colorIndex = 0;
            if (isXSSF)
            {
                excelColor = new XSSFColor(color);
            }
            else
            {
                HSSFPalette palette = ((HSSFWorkbook)workBook).GetCustomPalette();
                colorIndex = HSSFColor.Teal.Index;
                palette.SetColorAtIndex(colorIndex, color.R, color.G, color.B);
            }
        }
    }
}
