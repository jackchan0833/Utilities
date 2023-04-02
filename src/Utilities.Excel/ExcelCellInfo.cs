
namespace JC.Utilities.Excel
{
    /// <summary>
    /// Represents the excel cell info.
    /// </summary>
    public class ExcelCellInfo
    {
        /// <summary>
        /// The column index of excel cell.
        /// </summary>
        public int ColumnIndex { get; set; }

        /// <summary>
        /// The row index of excel cell.
        /// </summary>
        public int RowIndex { get; set; }

        /// <summary>
        /// The value of excel cell.
        /// </summary>
        public object Value { get; set; }

        /// <summary>
        /// The format of excel cell, default as <see cref="ExcelCellFormat.General"/> format.
        /// </summary>
        public ExcelCellFormat CellFormat { get; set; } = ExcelCellFormat.General;

        public ExcelCellInfo() { }
        public ExcelCellInfo(int columnIndex, int rowIndex, object value)
        {
            ColumnIndex = columnIndex;
            RowIndex = rowIndex;
            Value = value;
        }
    }
}
